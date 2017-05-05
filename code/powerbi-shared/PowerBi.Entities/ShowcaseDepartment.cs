using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class ShowcaseDepartment : IHaveSlug
    {
        public string Slug { get; set; }

        public string LocKey { get; set; }
    }
}
