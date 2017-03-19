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
                CommentID = c.LepraId;
                PostID = c.Post.LepraId;
                Rating = c.Rating;
                Body = c.Body;
                Code = c.VideoCode;
                Created = c.Created.ToString("dd-MM-yyyy HH:mm");
            }
        }

        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int Rating { get; set; }
        public string Body { get; set; }
        public string Code { get; set; }
        public string Created { get; set; }
    }
}