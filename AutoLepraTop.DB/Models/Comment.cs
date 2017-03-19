using System;

namespace AutoLepraTop.DB.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int LepraId { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string VideoCode { get; set; }
        public int Rating { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
        public int Post_Id { get; set; }

        public override bool Equals(object obj)
        {
            var comment = obj as Comment;
            return comment != null && comment.LepraId.Equals(LepraId);
        }

        public override int GetHashCode()
        {
            return LepraId.GetHashCode();
        }
    }
}