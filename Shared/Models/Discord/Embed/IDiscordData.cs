using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public interface IDiscordData : INewsData
    {
        string AuthorUrl { get; set; }
        string Description { get; set; }
        List<EmbedField> Fields { get; set; }
        string Type { get; set; }
    }
}