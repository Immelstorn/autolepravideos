using Newtonsoft.Json;

namespace AutoLepraTop.BL.Models
{
    public class Link
    {

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("params")]
        public object Parameters { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }
    }
}