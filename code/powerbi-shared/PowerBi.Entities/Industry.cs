using System;
using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    [Serializable]
    public class Industry : IHaveSlug, IHaveDetailPage
    {
        public string Slug { get; set; }

        public string Title_Lockey { get; set; }

        public string Icon { get; set; }

        public string IndexTagline_Lockey { get; set; }

        public string DetailTitle_Lockey { get; set; }

        public string DetailTagline_Lockey { get; set; }

        public string ReportThumbnail { get; set; }

        public IEnumerable<string> ReportImages { get; set; }

        public string EmbeddedReport { get; set; }

        public string Url
        {
            get { return string.Format("/industries/{0}/", this.Slug); }
        }
    }
}
