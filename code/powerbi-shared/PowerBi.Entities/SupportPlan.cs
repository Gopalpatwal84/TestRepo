using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class SupportPlan : IHaveSlug
    {
        public string Slug { get; set; }

        public string Id { get; set; }

        public string Sku { get; set; }

        public string Name { get; set; }

    }
}
