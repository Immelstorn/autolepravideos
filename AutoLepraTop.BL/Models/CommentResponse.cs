using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class CommentResponse
    {
        [JsonProperty("comments")]
        public IList<Comment> Comments { get; set; }
    }
}