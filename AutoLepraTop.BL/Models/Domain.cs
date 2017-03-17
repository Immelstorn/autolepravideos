using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class Domain
    {
        [JsonProperty("idna_url")]
        public string IdnaUrl { get; set; }

        [JsonProperty("is_in_bookmarks")]
        public bool IsInBookmarks { get; set; }

        [JsonProperty("is_mythings_subscribed")]
        public bool IsMythingsSubscribed { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("is_subscribed")]
        public bool IsSubscribed { get; set; }

        [JsonProperty("idna_prefix")]
        public string IdnaPrefix { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("is_voting_disabled")]
        public bool IsVotingDisabled { get; set; }

        [JsonProperty("id")]
        public int ID { get; set; }
    }
}