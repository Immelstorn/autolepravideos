using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class User
    {
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("is_ignored")]
        public bool IsIgnored { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }
    }
}