using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public interface INewsData
    {
        string AuthorName { get; set; }
        DateTimeOffset? TimeStamp { get; set; }
        string Title { get; set; }
        string Url { get; set; }
    }
}
