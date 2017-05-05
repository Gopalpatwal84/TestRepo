namespace PowerBI.Validation.Tests.Helpers
{
    using System.Collections.Generic;

    public class EmbeddedResource
    {
        public EmbeddedResource()
        {
            this.ResourceEntries = new List<CultureResources>();
        }

        public string Name { get; set; }

        public List<CultureResources> ResourceEntries { get; set; }
    }
}
