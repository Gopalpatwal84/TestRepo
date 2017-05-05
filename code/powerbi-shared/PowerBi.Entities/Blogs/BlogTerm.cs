namespace PowerBI.Entities.Blogs
{
    using System;
    using Acom.IO.Entities;

    [Serializable]
    public class BlogTerm : IHaveSlug
    {
        public string Slug { get; set; }

        public string Name { get; set; }
    }
}
