using System;
using Acom.DocumentDb;
using Acom.IO.Entities;
using Newtonsoft.Json;

namespace PowerBI.Jobs.Forms.Data
{
    [Serializable]
    public class SupportTicket : IHaveSlug, IHaveETag
    {
        public string Slug { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string TimeZone { get; set; }

        public string ProblemType { get; set; }

        public string Category { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string TenantId { get; set; }

        public string OrganizationName { get; set; }

        public string TicketId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
    }
}
