namespace PowerBI.Entities.Blogs
{
    using System;
    using Acom.IO.Entities;

    [Serializable]
    public class BlogMonthlyArchive : IHaveSlug, IHaveDetailPage
    {
        public string Url
        {
            get { return string.Format("/blog/{0:D4}/{1:D2}/", this.Year, this.Month); }
        }

        /// <summary>
        /// A slug with the format yyyy-mm
        /// </summary>
        public string Slug { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// The index of the first post for this month in the Sorted Set
        /// </summary>
        public int StartIndex { get; set; }
    }
}
