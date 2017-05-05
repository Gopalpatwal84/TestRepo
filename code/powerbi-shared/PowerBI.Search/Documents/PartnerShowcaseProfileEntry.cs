using System.Collections.Generic;
using Acom.Search.Client.Documents;
using Newtonsoft.Json;
using PowerBI.Entities;

namespace PowerBI.Search.Documents
{
    [ForEntity(typeof(PartnerShowcase))]
    public class PartnerShowcaseProfileEntry : PowerBiEntry
    {
        public PartnerShowcaseProfileEntry(string url, string slug)
            : base(url, slug)
        {
        }
        
        [JsonIgnore]
        public ExtraData ExtraPartnerData
        {
            get { return GetExtraData<ExtraData>(); }
            set { SetExtraData(value); }
        }

        public class ExtraData
        {
            public Dictionary<string, string> LogoUrls { get; set; }
        }
    }
}
