namespace PowerBI.Jobs.DocumentationFeedback.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Feedback
    {
        [Key]
        public Guid FeedbackId { get; set; }

        [Required]
        public string ArticleId { get; set; }

        [MaxLength(5), Required]
        public string Culture { get; set; }

        public string Hash { get; set; }

        public string GitHubSource { get; set; }

        public DateTime? ArticleLastModified { get; set; }

        public string Service { get; set; }

        [Required]
        public DateTime FeedbackTimestamp { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public bool Helpful { get; set; }

        public string IpAddress { get; set; }

        public string AdditionalIpAddresses { get; set; }

        public string Comment { get; set; }

        public string ControlVersion { get; set; }

        public string ExperimentId { get; set; }

        public string VariationId { get; set; }
    }
}
