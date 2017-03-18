﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private const int CommentsToSave = 250;

        private readonly string _token = "Bearer " + ConfigurationManager.AppSettings["token"];
        private readonly RestClient _client = new RestClient("https://leprosorium.ru/api/");

        public async void CheckAndUpdateDbIfNeeded()
        {
            using(var db = new AutoLepraTopDbContext())
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

                    await ParseVideos();
                }
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
                Debug.WriteLine($"Post {count++}");
                var r = new Regex("href=\"(?<link>.*?youtube.*?)\"");

                var comments = await GetPostComments(post.Id);
                var dbcomments = new List<DBComment>();
                Debug.WriteLine($"Comments {comments.Count}");
                foreach(var comment in comments)
                {
                    if(string.IsNullOrEmpty(comment.Body))
                    {
                        continue;
                    }
                    var match = r.Match(comment.Body);
                    if(match.Success)
                    {
                        var dbcomment = new DBComment {
                            Body = comment.Body,
                            Id = comment.ID,
                            Link = match.Groups[1].Value,
                            Rating = comment.Rating,
                            Created = UnixTimeStampToDateTime(comment.Created),
                            Post = post
                        };
                        dbcomments.Add(dbcomment);
                    }

                    //saving each 250 comments to avoid EF hanging
                    if(dbcomments.Count >= CommentsToSave)
                    {
                        Debug.WriteLine($"Inserting 250 dbcomments");
                        foreach(var dbcomment in dbcomments)
                        {
                            db.Comments.AddOrUpdate(dbcomment);
                        }
                        await db.SaveChangesWithIdentityAsync("Comments");
                        dbcomments.Clear();
                        db.Dispose();
                        db = new AutoLepraTopDbContext();
                    }
                }
                Debug.WriteLine($"Inserting {dbcomments.Count} dbcomments");

                foreach(var dbcomment in dbcomments)
                {
                    db.Comments.AddOrUpdate(dbcomment);
                }
                await db.SaveChangesWithIdentityAsync("Comments");
                db.Dispose();
                db = new AutoLepraTopDbContext();
            }
            Debug.WriteLine($"Done");
            Debug.WriteLine($"Time elapsed: {DateTime.Now - start}");
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }

        private async Task FindAllPostsAndSaveToDb()
        {
            var request = new RestRequest("users/tusinda/posts/");
            request.AddHeader("Authorization", _token);
            var page = 1;
            request.AddParameter("page", page);

            var posts = new List<Post>();
            var stop = false;
            //taking first tusinda's post with strange hobby
            while(!stop)
            {
                var param = request.Parameters.Find(p => p.Name.Equals("page"));
                param.Value = page;
                var response = await _client.ExecuteTaskAsync(request);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<UserResponse>(response.Content);
                    var videoPost = result.Posts.FirstOrDefault(p => p.Body != null && p.Body.ToLowerInvariant().Contains(Title));
                    if(videoPost != null)
                    {
                        posts.Add(videoPost);
                        stop = true;
                    }
                    page++;
                }
            }

            var comments = await GetPostComments(posts.First().ID);
            var previous = comments.First(c => c.Body.ToLowerInvariant().Contains("предыдущие посты")).Body;
            //parse ids of previous posts
            var r = new Regex("comments\\/(?<id>\\d*)[\\/#\"]");
            var matches = r.Matches(previous);

            var dbPosts = matches.Cast<Match>()
                    .Select(m => new DBPost {
                        Id = int.Parse(m.Groups[1].Value)
                    }).ToList();

            dbPosts.Add(new DBPost {
                Id = posts.First().ID
            });

            using(var db = new AutoLepraTopDbContext())
            {
                foreach (var dbPost in dbPosts.Distinct())
                {
                    if (await db.Posts.AnyAsync(p => p.Id.Equals(dbPost.Id)))
                    {
                        continue;
                    }
                    db.Posts.AddOrUpdate(dbPost);
                }
                await db.SaveChangesWithIdentityAsync("Posts");
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