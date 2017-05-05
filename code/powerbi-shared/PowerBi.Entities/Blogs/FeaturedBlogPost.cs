using System;
using Acom.IO.Entities;

namespace PowerBI.Entities.Blogs
{
    [Serializable]
    public class FeaturedBlogPost : IHaveSlug
    {
        public const string DefaultFeaturedSlug = "featured";

        /// <summary>
        /// Slug used to retrieve the blog posts from Redis.
        /// </summary>
        public string Slug
        {
            get { return DefaultFeaturedSlug; }
        }

        public BlogPost Post { get; set; }
    }
}
