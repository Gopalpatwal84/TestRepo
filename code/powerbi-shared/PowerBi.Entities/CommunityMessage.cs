using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class CommunityMessage : IHaveSlug
    {
        public string Slug { get; set; }

        public string Url { get; set; }

        public string Subject { get; set; }

        public string SearchSnippet { get; set; }

        public string Body { get; set; }

        public DateTime PostTime { get; set; }

        public DateTime LastEditTime { get; set; }
    }
}
