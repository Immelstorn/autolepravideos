using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class Comment
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty("unread")]
        public bool Unread { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("user_vote")]
        public object UserVote { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("can_moderate")]
        public bool CanModerate { get; set; }

        [JsonProperty("rating_order")]
        public int RatingOrder { get; set; }

        [JsonProperty("can_remove_comment_threads")]
        public bool CanRemoveCommentThreads { get; set; }

        [JsonProperty("parent_id")]
        public int? ParentID { get; set; }

        [JsonProperty("can_ban")]
        public bool CanBan { get; set; }

        [JsonProperty("tree_level")]
        public int TreeLevel { get; set; }

        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; }

        [JsonProperty("date_order")]
        public int DateOrder { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}