using System;
using System.Collections.Generic;

namespace PowerBI.Entities
{
    [Serializable]
    public class PartnerShowcaseProfile
    {
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string Culture { get; set; }
        public string SolutionName { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public IEnumerable<ShowcaseIndustry> Industries { get; set; }
        public IEnumerable<ShowcaseDepartment> Departments { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<PartnerCompetency> Competencies { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IEnumerable<string> Screenshots { get; set; }
        public string SolutionDescriptionVideo { get; set; }
        public string CaseStudyVideo { get; set; }
        public string ReportId { get; set; }
        public string PinpointUrl { get; set; }
        public string ContactUrl { get; set; }
    }
}
