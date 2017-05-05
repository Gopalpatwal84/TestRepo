namespace PowerBI.Jobs.Forms.Data
{
    using System;
    using System.Collections.Generic;
    using Acom.DocumentDb;
    using Acom.IO.Entities;
    using Newtonsoft.Json;

    [Serializable]
    public class User : IHaveSlug, IHaveETag
    {
        public User()
        {
            this.Forms = new List<string>();
        }

        public string Slug { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string JobTitle { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public string IpAddress { get; set; }

        public List<string> Forms { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
    }
}
