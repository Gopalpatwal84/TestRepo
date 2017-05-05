using PowerBI.Entities;
using System.Collections.Generic;

namespace PowerBI.Web.Models.Pages
{
    public class ViewMetadataModel
    {
        public ViewMetadataModel()
        {
            this.AdditionalMetaTags = new Dictionary<string, string>();
            this.SocialModel = new Social();
        }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaCanonical { get; set; }
        public Culture[] Cultures { get; set; }
        public Dictionary<string, string> AdditionalMetaTags { get; set; }
        public Social SocialModel { get; set; }
        public bool IsChinaCloud { get; set; }
    }
}