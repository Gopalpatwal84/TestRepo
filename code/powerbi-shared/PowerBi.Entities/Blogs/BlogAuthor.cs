namespace PowerBI.Entities.Blogs
{
    using System;
    using Acom.IO.Entities;

    [Serializable]
    public class BlogAuthor : IHaveSlug, IHaveDetailPage
    {
        public string Url
        {
            get { return string.Format("/blog/author/{0}/", this.Slug); }
        }

        public string Slug { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string ProfileImage { get; set; }
    }
}
