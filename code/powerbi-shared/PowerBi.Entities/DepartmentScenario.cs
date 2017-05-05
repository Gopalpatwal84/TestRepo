using System;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class DepartmentScenario : IHaveSlug
    {
        public string Slug { get; set; }

        public string TitleLockey { get; set; }

        public string Icon { get; set; }

        public string TaglineLockey { get; set; }

        public string DescriptionLockey { get; set; }

        public string EmbededReport { get; set; }

        public string Screenshot { get; set; }
    }
}
