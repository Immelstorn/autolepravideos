using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class UserResponse
    {
        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("posts")]
        public IList<Post> Posts { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
    }
}