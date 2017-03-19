using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLepraTop.DB.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int LepraId { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public override bool Equals(object obj)
        {
            var post = obj as Post;
            return post != null && post.LepraId.Equals(LepraId);
        }

        public override int GetHashCode()
        {
            return LepraId.GetHashCode();
        }
    }
}