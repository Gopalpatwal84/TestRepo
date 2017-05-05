using System.Linq;
using Acom.IO.Entities;
using PowerBI.Search.Documents;

namespace PowerBI.Web.Models.Pages.PartnerShowcases
{
    public class PartnerShowcaseSearchViewModel
    {
        public PartnerShowcaseProfileEntry Entry { get; set; }

        public string Name
        {
            get
            {
                return this.Entry.TitleLanguages[this.FirstAvailableProfileCulture] ?? this.Entry.Title;
            }
        }

        public string ShortDescription
        {
            get
            {
                return this.Entry.SummaryLanguages[this.FirstAvailableProfileCulture] ?? this.Entry.Summary;
            }
        }

        public string LogoUrl
        {
            get
            {
                string logo;
                if (!this.Entry.ExtraPartnerData.LogoUrls.TryGetValue(this.FirstAvailableProfileCulture, out logo))
                {
                    if (!this.Entry.ExtraPartnerData.LogoUrls.TryGetValue(KnownCultures.Default, out logo))
                    {
                        logo = this.Entry.ExtraPartnerData.LogoUrls.First().Value;
                    }
                }

                return $"/mediahandler/partnersv2/partnerfiles/{logo}";
            }
        }

        public string FirstAvailableProfileCulture
        {
            get
            {
                return this.Entry.Cultures.FirstOrDefault(x => x == KnownCultures.Current) ??
                       this.Entry.Cultures.FirstOrDefault(x => x == KnownCultures.Default) ??
                       this.Entry.Cultures.FirstOrDefault(x => x.StartsWith(KnownCultures.DefaultLanguage)) ??
                       this.Entry.Cultures.First();
            }
        }
    }
}