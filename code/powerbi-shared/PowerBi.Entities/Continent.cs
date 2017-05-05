using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Continent : IHaveSlug
    {
        public string Slug { get; set; }
        public string LocKey { get; set; }
        public string[] CountrySlugs { get; set; }

        public Country[] Countries { get; set; }
    }
}
