﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoLepraTop.DB.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public override bool Equals(object obj)
        {
            var post = obj as Post;
            return post != null && post.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}