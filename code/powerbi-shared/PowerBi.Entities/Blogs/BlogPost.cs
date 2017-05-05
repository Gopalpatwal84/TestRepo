namespace PowerBI.Entities.Blogs
{
    using System;
    using System.Collections.Generic;
    using Acom.IO.Entities;

    [Serializable]
    public class BlogPost : IHaveDetailPage, IHaveSlug
    {
        public string Url
        {
            get { return string.Format("/blog/{0}/", this.Slug); }
        }

        public string Slug { get; set; }

        public string Title { get; set; }

        public bool Featured { get; set; }

        public BlogAuthor Author { get; set; }

        public DateTimeOffset Published { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string Summary { get; set; }

        public IEnumerable<BlogTerm> Tags { get; set; }

        public IEnumerable<BlogTerm> Categories { get; set; }
    }
}
