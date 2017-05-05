using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class Curation<T> : IHaveSlug
    {
        public string Slug { get; set; }
        public IEnumerable<string> ItemSlugs { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
