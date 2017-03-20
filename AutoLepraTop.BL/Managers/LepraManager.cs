using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AutoLepraTop.API.Models;
using AutoLepraTop.BL.Models;
using AutoLepraTop.DB.Models;

using Newtonsoft.Json;

using RestSharp;

using Post = AutoLepraTop.BL.Models.Post;
using Comment = AutoLepraTop.BL.Models.Comment;

using DBPost = AutoLepraTop.DB.Models.Post;
using DBComment = AutoLepraTop.DB.Models.Comment;


namespace AutoLepraTop.BL.Managers
{
    public class LepraManager
    {
        private const string Title = "странного хобби";
        private const string LastUpdatedName = "LastUpdated";
        private const int CommentsToSave = 100;
        private const int PageSize = 25;

        private readonly string _token = "Bearer " + ConfigurationManager.AppSettings["token"];
        private readonly RestClient _client = new RestClient("https://leprosorium.ru/api/");

        public async void CheckAndUpdateDbIfNeeded()
        {
            using (var db = new AutoLepraTopDbContext())
            {
                var lastUpdatedSetting = await db.Settings.FirstOrDefaultAsync(s => s.Name.Equals(LastUpdatedName));
                var lastUpdated = lastUpdatedSetting == null ? DateTime.MinValue : DateTime.Parse(lastUpdatedSetting.Value);
                if(DateTime.UtcNow - lastUpdated >= TimeSpan.FromHours(24))
                {
                    if(lastUpdatedSetting == null)
                    {
                        db.Settings.Add(new Setting {
                            Name = LastUpdatedName,
                            Value = DateTime.UtcNow.ToString()
                        });
                    }
                    else
                    {
                        lastUpdatedSetting.Value = DateTime.UtcNow.ToString();
                    }
                    await db.SaveChangesAsync();
                    try
                    {
                        await ParseVideos();
                    }
                    catch (Exception e)
                    {
                        var ex = e;
                        Trace.TraceError(ex.Message);
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            Trace.TraceError(ex.Message);
                        }
                    }
                }
            }
        }

        public async Task<ListDto<CommentDto>> Get(int page, string sort)
        {
            using (var db = new AutoLepraTopDbContext())
            {
                var query = db.Comments as IQueryable<DBComment>;
                
                var count = await query.CountAsync();
                query = sort.ToLowerInvariant().Equals("bydate") 
                    ? query.OrderByDescending(c => c.Created) 
                    : query.OrderByDescending(c => c.Rating);

                query = query
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize);

                var comments = await query.ToListAsync();

                var result = new ListDto<CommentDto>
                {
                    Comments = comments.Select(c => new CommentDto(c)).ToList(),
                    Page = page,
                    TotalItems = count,
                    ItemsPerPage = PageSize,
                };
                return result;
            }
        }

        private async Task ParseVideos()
        {
            await FindAllPostsAndSaveToDb();

            var db = new AutoLepraTopDbContext();

            var posts = await db.Posts.ToListAsync();
            var count = 1;
            var start = DateTime.Now;
            foreach(var post in posts)
            {
                Trace.TraceInformation($"Post {count++}");
                var r = new Regex("href=\"(?<link>https?:\\/\\/(www.)?(m.)?(youtube|youtu\\.be).*?)\"");

                var comments = await GetPostComments(post.LepraId);
                var dbcomments = new List<DBComment>();
                Trace.TraceInformation($"Comments {comments.Count}");
                foreach(var comment in comments)
                {
                    if(string.IsNullOrEmpty(comment.Body))
                    {
                        continue;
                    }
                    var match = r.Match(comment.Body);
                    if(match.Success)
                    {
                        var link = match.Groups["link"].Value;

                        var code = GetCode(link);
                        if(!string.IsNullOrEmpty(code))
                        {
                            var dbcomment = new DBComment
                            {
                                Body = comment.Body,
                                LepraId = comment.ID,
                                Link = link,
                                VideoCode = code,
                                Rating = comment.Rating,
                                Created = UnixTimeStampToDateTime(comment.Created),
                                Post_Id = post.Id
                            };
                            dbcomments.Add(dbcomment);
                        }
                    }

//                    saving each 250 comments to avoid EF hanging
                    if(dbcomments.Count >= CommentsToSave)
                    {
                        Trace.TraceInformation($"Inserting {CommentsToSave} dbcomments");
                        foreach (var dbcomment in dbcomments)
                        {
                            var existing = await db.Comments.Where(c => c.LepraId.Equals(dbcomment.LepraId)).FirstOrDefaultAsync();
                            if (existing == null)
                            {
                                db.Comments.Add(dbcomment);
                            }
                            else
                            {
                                existing.Rating = dbcomment.Rating;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbcomments.Clear();
                        db.Dispose();
                        db = new AutoLepraTopDbContext();
                    }
                }
                Trace.TraceInformation($"Inserting {dbcomments.Count} dbcomments");

                foreach(var dbcomment in dbcomments)
                {
                    var existing = await db.Comments.Where(c => c.LepraId.Equals(dbcomment.LepraId)).FirstOrDefaultAsync();
                    if(existing == null)
                    {
                        db.Comments.Add(dbcomment);
                    }
                    else
                    {
                        existing.Rating = dbcomment.Rating;
                    }
                }

                await db.SaveChangesAsync();

                db.Dispose();
                db = new AutoLepraTopDbContext();
            }
            Debug.WriteLine($"Done");
            Debug.WriteLine($"Time elapsed: {DateTime.Now - start}");
        }

        private static string GetCode(string link)
        {
            var code = string.Empty;
            if(link.Contains("v="))
            {
                var v = link.Substring(link.IndexOf("v=", StringComparison.Ordinal) + "v=".Length);
                code = v.Contains("&") ? v.Substring(0, v.IndexOf("&", StringComparison.Ordinal)) : v;
            }
            else if(link.Contains("/embed/"))
            {
                code = link.Substring(link.IndexOf("/embed/", StringComparison.Ordinal) + "/embed/".Length);
            }
            else if(link.Contains("youtu.be/"))
            {
                var v = link.Substring(link.IndexOf("youtu.be/", StringComparison.Ordinal) + "youtu.be/".Length);
                code = v.Contains("?") ? v.Substring(0, v.IndexOf("?", StringComparison.Ordinal)) : v;
            }
            else if(link.Contains("/user/") && link.Contains("#"))
            {
                var lastSlash = link.LastIndexOf("/", StringComparison.Ordinal);
                code = link.Substring(lastSlash + 1, link.Length - lastSlash - 1);
            }
            return code;
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        private async Task FindAllPostsAndSaveToDb()
        {
            //            var request = new RestRequest("users/tusinda/posts/");
            //            request.AddHeader("Authorization", _token);
            //            var page = 1;
            //            request.AddParameter("page", page);
            //
            //            var posts = new List<Post>();
            //            var stop = false;
            //            //taking first tusinda's post with strange hobby
            //            while(!stop)
            //            {
            //                var param = request.Parameters.Find(p => p.Name.Equals("page"));
            //                param.Value = page;
            //                var response = await _client.ExecuteTaskAsync(request);
            //                if(response.StatusCode == HttpStatusCode.OK)
            //                {
            //                    var result = JsonConvert.DeserializeObject<UserResponse>(response.Content);
            //                    var videoPost = result.Posts.FirstOrDefault(p => p.Body != null && p.Body.ToLowerInvariant().Contains(Title));
            //                    if(videoPost != null)
            //                    {
            //                        posts.Add(videoPost);
            //                        stop = true;
            //                    }
            //                    page++;
            //                }
            //            }
            //            var dbPosts = new List<DBPost>();
            //            var comments = await GetPostComments(posts.First().ID);
            //            var previous = comments.FirstOrDefault(c => c.Body.ToLowerInvariant().Contains("предыдущие посты"))?.Body;
            //            //parse ids of previous posts
            //            if(previous != null)
            //            {
            //                var r = new Regex("comments\\/(?<id>\\d*)[\\/#\"]");
            //                var matches = r.Matches(previous);
            //
            //                dbPosts.AddRange(matches.Cast<Match>()
            //                        .Select(m => new DBPost {
            //                            Id = int.Parse(m.Groups["id"].Value)
            //                        }).ToList());
            //            }
            //
            //            dbPosts.Add(new DBPost {
            //                Id = posts.First().ID
            //            });

            var dbPosts = PostsNumbers.Posts.Select(p => new DBPost {
                LepraId = p
            }).Distinct().ToList();


            using (var db = new AutoLepraTopDbContext())
            {
                foreach (var dbPost in dbPosts)
                {
                    if (await db.Posts.AnyAsync(p => p.LepraId.Equals(dbPost.LepraId)))
                    {
                        continue;
                    }
                    db.Posts.Add(dbPost);
                }

                await db.SaveChangesAsync();
            }
        }

        private async Task<List<Comment>> GetPostComments(int id)
        {
            var request = new RestRequest($"posts/{id}/comments/");
            request.AddHeader("Authorization", _token);
            var comments = new List<Comment>();

            var response = await _client.ExecuteGetTaskAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<CommentResponse>(response.Content);
                comments.AddRange(result.Comments);
            }
            
            return comments;
        }
    }
}