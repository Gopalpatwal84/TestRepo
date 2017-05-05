using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class ProblemCategory : IHaveSlug
    {
        /// <summary>
        /// Gets or sets the slug.
        /// </summary>
        public string Slug { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public Dictionary<string, string> AlternateCultures { get; set; }
    }
}
