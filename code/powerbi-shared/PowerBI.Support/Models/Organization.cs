namespace PowerBI.Support.Models
{
    public class Organization
    {
        public string TenantId { get; set; }

        public string Name { get; set; }

        public string RelationType { get { return "Customer"; } }
    }
}