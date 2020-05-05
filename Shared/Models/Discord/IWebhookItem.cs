using System.Collections.Generic;

namespace Shared.Models
{
    public interface IWebhookItem
    {
        string AvatarUrl { get; set; }
        string Content { get; set; }
        List<Embed> Embeds { get; set; }
        bool IsTTS { get; set; }
        string Username { get; set; }
    }
}