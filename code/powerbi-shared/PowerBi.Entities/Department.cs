using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Department : IHaveSlug, IHaveDetailPage
    {
        public string Slug { get; set; }

        public string TitleLockey { get; set; }

        public string Icon { get; set; }

        public string IndexTaglineLockey { get; set; }

        public string DetailTaglineLockey { get; set; }

        public string DescriptionLockey { get; set; }

        public string Screenshot { get; set; }

        public IEnumerable<DepartmentScenario> Scenarios { get; set; }

        public string Url
        {
            get { return string.Format("/departments/{0}/", this.Slug); }
        }
    }
}
