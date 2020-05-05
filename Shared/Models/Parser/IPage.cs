using System;

namespace Shared.Models
{
    public interface IPage
    {
        PageAuthor Author { get; set; }
        string PageUrl { get; set; }
        string Title { get; set; }
        DateTime UpdateTime { get; set; }
    }
}