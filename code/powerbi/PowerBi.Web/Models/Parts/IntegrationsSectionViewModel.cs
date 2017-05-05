using System.Collections.Generic;
using PowerBI.Entities;

namespace PowerBI.Web.Models.Parts
{
    public class IntegrationsSectionViewModel
    {
        public IEnumerable<Integration> AllIntegrations { get; set; }

        public IEnumerable<Integration> FeaturedIntegrations { get; set; }

        public Integration Integration { get; set; }
    }
}