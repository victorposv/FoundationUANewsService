using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class TelegramData : ITelegramData
    {
        [JsonProperty]
        public string AuthorName { get; set; }
        [JsonProperty]
        public DateTimeOffset? TimeStamp { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Url { get; set; }
    }
}
