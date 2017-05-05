using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.Partners
{
    public class PartnerDetailsModel : ViewMetadataModel
    {
        public Partner Partner { get; set; }

        public PartnerSearchViewModel[] FeaturedPartners { get; set; }

        public bool InPreview { get; set; }
    }
}