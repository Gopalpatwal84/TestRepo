using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class PinpointPartner : IHaveSlug
    {
        public string Slug { get; set; }

        public string PinpointId { get; set; }

        public string Culture { get; set; }

        public string CountryCode { get; set; }
    }
}
