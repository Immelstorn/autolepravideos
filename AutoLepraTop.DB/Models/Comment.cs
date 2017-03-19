using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLepraTop.DB.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public int Rating { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
        public int Post_Id { get; set; }

        public override bool Equals(object obj)
        {
            var comment = obj as Comment;
            return comment != null && comment.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}