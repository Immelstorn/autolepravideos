using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;

using AutoLepraTop.BL.Models;

using Newtonsoft.Json;

using RestSharp;

namespace AutoLepraTop.BL.Managers
{
    public class LepraManager
    {
        private const string Title = "странного хобби";

        private readonly string _token = "Bearer " + ConfigurationManager.AppSettings["token"];
        private readonly RestClient _client = new RestClient("https://leprosorium.ru/api/");

        public object GetAllPostsFromAuto()
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
                var param = request.Parameters.Find(p=>p.Name.Equals("page"));
                param.Value = page;
                var response = _client.Execute(request);
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

            var comments = GetPostComments(posts.First().ID);
            var previous = comments.First(c => c.Body.ToLowerInvariant().Contains("предыдущие посты")).Body;
            //parse ids of previous posts

            return null;
        }

        private List<Comment> GetPostComments(int id)
        {
            var request = new RestRequest($"posts/{id}/comments/");
            request.AddHeader("Authorization", _token);
            var comments = new List<Comment>();

            var response = _client.Execute(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<CommentResponse>(response.Content);
                comments.AddRange(result.Comments);
            }
            
            return comments;
        }
    }
}