namespace PowerBI.Validation.Tests.Helpers
{
    using System.Collections.Generic;

    public class CultureResources
    {
        public CultureResources()
        {
            this.Entries = new Dictionary<string, string>();
        }

        public string Culture { get; set; }

        public Dictionary<string, string> Entries { get; set; }
    }
}
