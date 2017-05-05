using System.Collections.Generic;

namespace PowerBI.Support.Models
{
    public class Entitlement
    {
        public Entitlement()
        {
            this.ExtendedProperties = new List<KeyValuePair<string, string>>();
        }

        public string EntitlementType { get { return "1"; } }

        public string EntitlementId { get { return ""; } }

        public List<KeyValuePair<string,string>> ExtendedProperties { get; set; }
    }
}