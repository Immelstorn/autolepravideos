using System.Collections.Generic;

using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class DomainRespose
    {
        [JsonProperty("posts")]
        public IList<Post> Posts { get; set; }
    }
}