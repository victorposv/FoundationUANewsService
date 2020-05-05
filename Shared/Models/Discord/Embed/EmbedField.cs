using Newtonsoft.Json;

namespace Shared.Models
{
    [JsonObject]
    public class EmbedField : IEmbedField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("inline")]
        public bool Inline { get; set; }
    }
}
