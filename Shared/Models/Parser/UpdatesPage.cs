using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class UpdatesPage : IPage
    {
        public string Title { get; set; }
        public PageAuthor Author { get; set; }
        public DateTime UpdateTime { get; set; }
        public string PageUrl { get; set; }
    }
}
