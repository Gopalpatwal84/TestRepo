using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Course : IHaveSlug
    {
        public string Slug { get; set; }
        public string TitleLocKey { get; set; }
        public string Icon { get; set; }
        public IEnumerable<string> ArticleSlugs { get; set; }
    }
}
