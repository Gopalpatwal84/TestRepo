using System;
using System.Linq;
using System.Threading.Tasks;
using Acom.Configuration;
using Mediation.Handlers;
using PowerBI.Data;
using PowerBI.Jobs.DocumentationFeedback.Data;
using PowerBI.Jobs.DocumentationFeedback.Data.Models;
using PowerBI.Messages.Commands;
using PowerBI.Messages.Models;
using Serilog;

namespace PowerBI.Jobs.DocumentationFeedback
{
    public class DocumentationFeedbackHandler : IHandleCommand<ArticleFeedbackMessage>
    {
        private readonly DocumentationFeedbackDbDataService _dataService;
        private readonly ILogger _logger;

        public DocumentationFeedbackHandler(ILogger logger, DocumentationFeedbackDbDataService dataService)
        {
            this._logger = logger;
            this._dataService = dataService;
        }

        public async Task Handle(ArticleFeedbackMessage message)
        {
            _logger.Debug("Processing articles feedback for article {ArticleId} (culture: {Culture})...", message.ArticleId, message.Culture);
            _logger.Debug("Was article {ArticleId} helpful?: {Helpful}", message.ArticleId, message.Helpful);

            var feedbackIpAddress = !string.IsNullOrEmpty(message.IpAddress) ?
                message.IpAddress.Split(',') : new string[0];

            var feedback = new Feedback
            {
                FeedbackId = message.CorrelationId != Guid.Empty ? message.CorrelationId : message.DiagnosticInfo(x => x.MessageId, Guid.NewGuid()),
                IpAddress = feedbackIpAddress.FirstOrDefault(),
                AdditionalIpAddresses = feedbackIpAddress.Count() > 1 ? string.Join(",", feedbackIpAddress.Skip(1)) : null,
                ArticleId = message.ArticleId,
                ArticleLastModified = message.ArticleLastModified.AsSqlDate(),
                Comment = message.Comment,
                Culture = message.Culture,
                FeedbackTimestamp = message.FeedbackTimestamp,
                GitHubSource = message.GitHubSource,
                Hash = message.Hash,
                Helpful = message.Helpful,
                Service = message.Service,
                Url = message.Url
            };

            _dataService.SaveFeedback(feedback);

            _logger.Information("Processing articles feedback for article {ArticleId} completed.", message.ArticleId);
        }
    }
}