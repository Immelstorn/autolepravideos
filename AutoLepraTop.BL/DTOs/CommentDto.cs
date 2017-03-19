using System;

using DBComment = AutoLepraTop.DB.Models.Comment;

namespace AutoLepraTop.BL.Models
{
    public class CommentDto
    {
        public CommentDto(DBComment c)
        {
            if(c != null)
            {
                CommentID = c.Id;
                PostID = c.Post_Id;
                Rating = c.Rating;
                Body = c.Body;
                Created = c.Created;
            }
        }

        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
    }
}