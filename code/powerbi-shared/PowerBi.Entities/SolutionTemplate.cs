namespace PowerBI.Entities
{
    using System.Collections.Generic;
    using Acom.IO.Entities;

    public class SolutionTemplate : IHaveSlug, IHaveDetailPage
    {
        public string Slug { get; set; }

        public string Title_Lockey { get; set; }

        public string Partner_Lockey { get; set; }

        public string Icon { get; set; }

        public string IndexTagline_Lockey { get; set; }

        public string DetailHeroTitle_LockKey { get; set; }

        public string DetailHeroTagline_Lockey { get; set; }

        public string DetailTitle_LockKey { get; set; }

        public string DetailTagline_Lockey { get; set; }

        public string DetailContent_Lockey { get; set; }

        public string ReportThumbnail { get; set; }

        public IEnumerable<string> ReportImages { get; set; }

        public IEnumerable<PowerBIPartner> FeaturedPartners { get; set; }

        public IEnumerable<string> FeaturedPartnerSlugs { get; set; }

        public IEnumerable<PowerBIPartner> EtlPartners { get; set; }

        public IEnumerable<string> EtlPartnerSlugs { get; set; }

        public string EmbeddedReport { get; set; }

        public string Url
        {
            get { return string.Format("/solution-templates/{0}/", this.Slug); }
        }

        public string PartnerUrl { get; set; }

        public string GitHubUrl { get; set; }

        public string DocumentationUrl { get; set; }

        public string YoutubeVideoId { get; set; }

        public string InstallNowUrl { get; set; }
    }
}
