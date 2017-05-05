using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.PartnerShowcases
{
    public class PartnerShowcaseDetailsViewModel : ViewMetadataModel
    {
        public PartnerShowcase Partner { get; set; }

        public bool InPreview { get; set; }
    }
}