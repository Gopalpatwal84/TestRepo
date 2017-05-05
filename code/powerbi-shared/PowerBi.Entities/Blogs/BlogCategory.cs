namespace PowerBI.Entities.Blogs
{
    using System;
    using Acom.IO.Entities;

    [Serializable]
    public class BlogCategory : BlogTerm, IHaveDetailPage
    {
        public string Url
        {
            get { return string.Format("/blog/category/{0}/", this.Slug); }
        }

        public int Count { get; set; }
    }
}
