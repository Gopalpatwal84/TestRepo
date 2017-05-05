using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class PowerBIPartner : IHaveSlug
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
