namespace PowerBI.Messages.Commands
{
    using System;
    using Mediation.Queues;

    [DestinationQueueName("articles-feedback")]
    public class ArticleFeedbackMessage : DiagnosticsCommand
    {
        public string ArticleId { get; set; }

        public string Culture { get; set; }

        public string Hash { get; set; }

        public string GitHubSource { get; set; }

        public DateTime? ArticleLastModified { get; set; }

        public string Service { get; set; }

        public DateTime FeedbackTimestamp { get; set; }

        public string Url { get; set; }

        public bool Helpful { get; set; }

        public string IpAddress { get; set; }

        public string Comment { get; set; }

        public Guid CorrelationId { get; set; }

        public string ControlVersion { get; set; }

        public string ExperimentId { get; set; }

        public string VariationId { get; set; }
    }
}
