using System;
using System.Collections.Generic;

namespace PowerBI.Entities
{
    [Serializable]
    public class PartnerProfile
    {
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
