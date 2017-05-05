using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Partner : IHaveSlug
    {
        public string Slug { get; set; }
        public IEnumerable<PartnerProfile> Profiles { get; set; }
        public string Url => $"/partners/{Slug}/";
    }
}
