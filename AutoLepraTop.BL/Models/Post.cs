using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class Post
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty("domain")]
        public Domain Domain { get; set; }

        [JsonProperty("in_interests")]
        public bool InInterests { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("raw_body")]
        public string RawBody { get; set; }

        [JsonProperty("can_moderate")]
        public bool CanModerate { get; set; }

        [JsonProperty("unread_comments_count")]
        public int UnreadCommentsCount { get; set; }

        [JsonProperty("golden")]
        public bool Golden { get; set; }

        [JsonProperty("vote_weight")]
        public int VoteWeight { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("pinned")]
        public bool Pinned { get; set; }

        [JsonProperty("user_vote")]
        public int? UserVote { get; set; }

        [JsonProperty("can_ban")]
        public bool CanBan { get; set; }

        [JsonProperty("_links")]
        public IList<Link> Links { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; }

        [JsonProperty("in_favourites")]
        public bool InFavourites { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        public override bool Equals(object obj)
        {
            var post = obj as Post;
            return post != null && ID.Equals(post.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}