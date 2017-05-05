using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class PartnerShowcase : IHaveSlug, IHaveDetailPage
    {
        public string Slug { get; set; }

        public IEnumerable<PartnerShowcaseProfile> Profiles { get; set; }

        public string Url
        {
            get { return string.Format("/partner-showcase/{0}/", this.Slug); }
        }
    }
}
