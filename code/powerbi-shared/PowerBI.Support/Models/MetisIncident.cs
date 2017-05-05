namespace PowerBI.Support.Models
{
    public class MetisIncident
    {
        public MetisIncident()
        {
            this.PrimaryContact = new PrimaryContact();
            this.Organization = new Organization();
            this.Entitlement = new Entitlement();
        }

        public string Title { get { return string.Format("Power BI Pro Limited - {0}", this.Subject); } }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Severity { get { return "SeverityC"; } }

        public string CustomerCountry { get; set; }

        public string ProductId => "16126";

        public string SupportTopicId { get; set; }

        public PrimaryContact PrimaryContact { get; set; }

        public Entitlement Entitlement { get; set; }

        public Organization Organization { get; set; }
    }
}
