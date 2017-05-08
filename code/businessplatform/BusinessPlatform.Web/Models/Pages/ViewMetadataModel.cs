namespace BusinessPlatform.Web.Models.Pages
{
    using System.Collections.Generic;
    using Entities;

    public class ViewMetadataModel
    {
        public ViewMetadataModel()
        {
            this.AdditionalMetaTags = new Dictionary<string, string>();
        }

        public string PageTitle { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaCanonical { get; set; }

        public Culture[] Cultures { get; set; }

        public Dictionary<string, string> AdditionalMetaTags { get; set; }
    }
}