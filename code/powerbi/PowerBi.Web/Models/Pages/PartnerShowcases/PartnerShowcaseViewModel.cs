using System.Collections.Generic;
using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.PartnerShowcases
{
    public class PartnerShowcaseViewModel : ViewMetadataModel
    {
        public IEnumerable<PartnerShowcaseSearchViewModel> Partners { get; set; }

        public IEnumerable<PartnerShowcaseSearchViewModel> FeaturedPartners { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public IEnumerable<ShowcaseIndustry> Industries { get; set; }

        public IEnumerable<ShowcaseDepartment> Departments { get; set; }

        public string SelectedCountry { get; set; }

        public string SelectedIndustry { get; set; }

        public string SelectedDepartment { get; set; }

        public string SearchTerm { get; set; }
    }
}