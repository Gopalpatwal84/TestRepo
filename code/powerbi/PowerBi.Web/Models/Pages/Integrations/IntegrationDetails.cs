namespace PowerBI.Web.Models.Pages.Integrations
{
    using System.Collections.Generic;
    using PowerBI.Entities;

    public class IntegrationDetails : ViewMetadataModel
    {
        public Integration Integration { get; set; }

        public IEnumerable<Integration> IntegrationsList { get; set; }
    }
}