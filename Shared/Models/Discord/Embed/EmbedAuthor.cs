using Newtonsoft.Json;

namespace Shared.Models
{
    [JsonObject]
    public class EmbedAuthor : IEmbedAuthor
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
