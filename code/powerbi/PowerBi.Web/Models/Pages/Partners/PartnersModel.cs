using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.Partners
{
    public class PartnersModel : ViewMetadataModel
    {
        public PartnerSearchViewModel[] Partners { get; set; }
        public PartnerSearchViewModel[] FeaturedPartners { get; set; }
        public Country[] Countries { get; set; }
        public string SelectedCountry { get; set; }
        public Pagination Pagination { get; set; }
    }
}