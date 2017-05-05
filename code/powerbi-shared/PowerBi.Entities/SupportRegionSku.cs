using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class SupportRegionSku : IHaveSlug
    {
        public string Slug { get; set; }

        public string ProductServicePackageSku { get; set; }

        public string Region { get; set; }

        public IEnumerable<string> Countries { get; set; }
    }
}
