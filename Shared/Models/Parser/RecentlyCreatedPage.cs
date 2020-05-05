using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class RecentlyCreatedPage : IPage
    {
        public PageAuthor Author { get; set; }
        public string PageUrl { get; set; }
        public string Title { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
