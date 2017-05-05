using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class PartnerCompetency : IHaveSlug
    {
        public string Slug { get; set; }

        public string Competency { get; set; }

        public string Level { get; set; }
    }
}
