namespace PowerBI.Support.Models
{
    public class PrimaryContact
    {
        public string Puid { get; set; }

        public string AadObjectId { get; set; }

        public string OrganizationTenantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PrimaryEmailAddress { get; set; }

        public string PrimaryPhoneNumber { get; set; }

        public string TimeZoneId { get; set; }

        public int TimeZone { get { return 0; } }

        public string Language { get; set; }

        public string PreferredContactMethod { get { return "None"; } }
    }
}