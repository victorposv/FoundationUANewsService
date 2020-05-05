using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class Embed : IDiscordData
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "rich";
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("timestamp")]
        public DateTimeOffset? TimeStamp { get; set; }
        [JsonProperty("author")]
        public EmbedAuthor Author { get; set; }
        [JsonProperty("fields")]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();
        [JsonIgnore]
        public string AuthorName { get { return Author.Name; } set { Author.Name = value; } }
        [JsonIgnore]
        public string AuthorUrl { get { return Author.Url; } set { Author.Url = value; } }

        public Embed()
        {
            Author = new EmbedAuthor();
        }
    }
}
