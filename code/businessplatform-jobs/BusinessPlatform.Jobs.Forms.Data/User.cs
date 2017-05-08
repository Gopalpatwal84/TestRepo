namespace BusinessPlatform.Jobs.Forms.Data
{
    using System;
    using System.Collections.Generic;
    using Acom.DocumentDb;
    using Acom.IO.Entities;

    public class User : IHaveSlug
    {
        public User()
        {
            this.Forms = new List<string>();
        }

        public string Slug { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public List<string> Forms { get; set; }
    }
}
