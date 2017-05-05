using System;
using Acom.IO.Entities;

namespace PowerBI.Entities.Blogs
{
    [Serializable]
    public class BlogContent : IHaveSlug
    {
        public string Slug { get; set; }
        public string Content { get; set; }
    }
}
