namespace PowerBI.Jobs.Forms.DocumentationFeedback.FunctionalTests
{
    using System;
    using System.Threading;
    using Autofac;
    using Jobs.DocumentationFeedback.Data.Models;
    using Mediation.Queues;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using PowerBI.Jobs.DocumentationFeedback.Data;
    using PowerBI.Jobs.DocumentationFeedback.Data.Context;
    using PowerBI.Messages.Commands;
    using PowerBI.Messages.Models;

    [TestClass]
    public class DocumentationFeedbackSyntheticTransactionFixture
    {
        private const string DummyArticleId = "test-synthetic-transaction-article";
        private const string RunningStatus = "Running";

        private static DateTime timestamp = DateTime.UtcNow;
        private static Configuration configuration;

        // Used for clean-up.
        private static Guid? entityToRemoveGuid;
        private static DynamicTableEntity tableEntityToRemove;
        private static IContainer container;

        [ClassInitialize]
        public static void LoadConfigurations(TestContext context)
        {
            entityToRemoveGuid = null;
            tableEntityToRemove = null;
            configuration = new Configuration();

            var builder = new ContainerBuilder();
            container = builder.Build();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (configuration.DoCleanUp)
            {
                if (entityToRemoveGuid != null && entityToRemoveGuid.HasValue)
                {
                    var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

                    var entity = dataService.GetFeedback(entityToRemoveGuid.Value);
                    dataService.DeleteFeedback(entity);
                }

                if (tableEntityToRemove != null)
                {
                    CloudTable table = GetMessageDiagnosticsInfoTable();

                    TableOperation deleteOperation = TableOperation.Delete(tableEntityToRemove);
                    table.Execute(deleteOperation);
                }
            }
        }

        [TestMethod]
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSavedInTheDatabase_WithoutCorrelationId()
        {
            // Arrange
            ArticleFeedbackMessage message = this.CreateFeedbackMessage();
            var diagMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Act
            mediator.Send(message).Wait();

            // Assert
            Feedback feedback = GetFeedbackFromDb(dataService, diagMessage.MessageId);

            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveGuid = feedback.FeedbackId;

            // We are not asserting dates due to possible differences between time zones.
            Assert.AreEqual(diagMessage.MessageId, feedback.FeedbackId);
            Assert.AreEqual(message.ArticleId, feedback.ArticleId);
            Assert.AreEqual(message.Culture, feedback.Culture);
            Assert.AreEqual(message.Hash, feedback.Hash);
            Assert.AreEqual(message.GitHubSource, feedback.GitHubSource);
            Assert.AreEqual(message.Service, feedback.Service);
            Assert.AreEqual(message.Url, feedback.Url);
            Assert.AreEqual(message.Helpful, feedback.Helpful);
            Assert.AreEqual(message.IpAddress, feedback.IpAddress);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSavedInTheDatabase_WithCorrelationId()
        {
            // Arrange
            ArticleFeedbackMessage message = this.CreateFeedbackMessage();
            var correlationId = Guid.NewGuid();
            message.CorrelationId = correlationId;
            var diagMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Act
            mediator.Send(message).Wait();

            // Assert
            Feedback feedback = GetFeedbackFromDb(dataService, message.CorrelationId);

            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveGuid = feedback.FeedbackId;

            // We are not asserting dates due to possible differences between time zones.
            Assert.AreEqual(correlationId, feedback.FeedbackId);
            Assert.AreEqual(message.ArticleId, feedback.ArticleId);
            Assert.AreEqual(message.Culture, feedback.Culture);
            Assert.AreEqual(message.Hash, feedback.Hash);
            Assert.AreEqual(message.GitHubSource, feedback.GitHubSource);
            Assert.AreEqual(message.Service, feedback.Service);
            Assert.AreEqual(message.Url, feedback.Url);
            Assert.AreEqual(message.Helpful, feedback.Helpful);
            Assert.AreEqual(message.IpAddress, feedback.IpAddress);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void QueueMessageWithExistentCorrelationId_ShouldUpdateFeedbackEntry()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var comment = string.Format("Testing feedback message. {0}", DateTime.UtcNow);

            ArticleFeedbackMessage voteNoMsg = this.CreateFeedbackMessage();
            voteNoMsg.Helpful = false;
            voteNoMsg.CorrelationId = correlationId;

            ArticleFeedbackMessage commentMsg = this.CreateFeedbackMessage();
            commentMsg.Helpful = false;
            commentMsg.CorrelationId = correlationId;
            commentMsg.Comment = comment;

            var diagMessage = (IDiagnosticInfo)voteNoMsg;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Act
            mediator.Send(voteNoMsg).Wait();

            Feedback feedback = GetFeedbackFromDb(dataService, voteNoMsg.CorrelationId);

            mediator.Send(commentMsg).Wait();

            dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Avoid returning cached entries
            feedback = GetFeedbackFromDb(dataService, commentMsg.CorrelationId, true);

            // Assert
            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveGuid = feedback.FeedbackId;

            // We are not asserting dates due to possible differences between time zones.
            Assert.AreEqual(correlationId, feedback.FeedbackId);
            Assert.AreEqual(comment, feedback.Comment);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void QueueTwoMessagesWithSameCorrelationId_ShouldStorageOneFeedbackEntry()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var comment = string.Format("Testing feedback message. {0}", DateTime.UtcNow);

            ArticleFeedbackMessage voteNoMsg = this.CreateFeedbackMessage();
            voteNoMsg.Helpful = false;
            voteNoMsg.CorrelationId = correlationId;
            
            ArticleFeedbackMessage commentMsg = this.CreateFeedbackMessage();
            commentMsg.Helpful = false;
            commentMsg.CorrelationId = correlationId;
            commentMsg.Comment = comment;
            
            var diagMessage = (IDiagnosticInfo)voteNoMsg;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Act
            mediator.Send(voteNoMsg).Wait();
            mediator.Send(commentMsg).Wait();

            // Assert
            Feedback feedback = GetFeedbackFromDb(dataService, voteNoMsg.CorrelationId, true);

            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveGuid = feedback.FeedbackId;

            // We are not asserting dates due to possible differences between time zones.
            Assert.AreEqual(correlationId, feedback.FeedbackId);
            Assert.AreEqual(voteNoMsg.ArticleId, feedback.ArticleId);
            Assert.AreEqual(voteNoMsg.Culture, feedback.Culture);
            Assert.AreEqual(voteNoMsg.Hash, feedback.Hash);
            Assert.AreEqual(voteNoMsg.GitHubSource, feedback.GitHubSource);
            Assert.AreEqual(voteNoMsg.Service, feedback.Service);
            Assert.AreEqual(voteNoMsg.Url, feedback.Url);
            Assert.AreEqual(voteNoMsg.Helpful, feedback.Helpful);
            Assert.AreEqual(voteNoMsg.IpAddress, feedback.IpAddress);
            Assert.AreEqual(comment, feedback.Comment);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void QueueFeedbackCommentFirst_NegativeVoteLast_ShouldAddOneFeedbackEntryWithTheComment()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var comment = string.Format("Testing feedback message. {0}", DateTime.UtcNow);

            ArticleFeedbackMessage voteNoMsg = this.CreateFeedbackMessage();
            voteNoMsg.Helpful = false;
            voteNoMsg.CorrelationId = correlationId;

            ArticleFeedbackMessage commentMsg = this.CreateFeedbackMessage();
            commentMsg.Helpful = false;
            commentMsg.CorrelationId = correlationId;
            commentMsg.Comment = comment;

            var diagMessage = (IDiagnosticInfo)voteNoMsg;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());

            // Act
            mediator.Send(commentMsg).Wait();

            Feedback feedback = GetFeedbackFromDb(dataService, commentMsg.CorrelationId);

            mediator.Send(voteNoMsg).Wait();

            // Avoid returning cached entries
            dataService = new DocumentationFeedbackDbDataService(new DocumentationFeedbackDbContext());
            
            feedback = GetFeedbackFromDb(dataService, voteNoMsg.CorrelationId, true);

            // Assert
            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveGuid = feedback.FeedbackId;

            // We are not asserting dates due to possible differences between time zones.
            Assert.AreEqual(correlationId, feedback.FeedbackId);
            Assert.AreEqual(comment, feedback.Comment);

            AssertDiagnosticInfo(diagMessage);
        }

        private static void AssertDiagnosticInfo(IDiagnosticInfo diagMessage)
        {
            // We also want to check that the diagnostics information for this message was logged in the storage table.
            // At this point we already know that the message was processed, so we don't need to wait for the info to appear in the table.
            CloudTable table = GetMessageDiagnosticsInfoTable();

            Assert.IsTrue(table.Exists());

            TableOperation retrive = TableOperation.Retrieve<DynamicTableEntity>(diagMessage.DiagnosticsInfo.SentTimestamp.ToString("yyyy-MM-dd"), diagMessage.MessageId.ToString());
            TableResult tableResult = table.Execute(retrive);

            Assert.AreEqual(200, tableResult.HttpStatusCode);

            DynamicTableEntity entity = tableResult.Result as DynamicTableEntity;
            Assert.IsNotNull(entity);

            // Holds a reference to the table entity to remove it during clean-up.
            tableEntityToRemove = entity;

            Assert.AreEqual(diagMessage.DiagnosticsInfo.DeploymentVersion, entity.Properties["DeploymentVersion"].StringValue);
            Assert.AreEqual(diagMessage.DiagnosticsInfo.InstanceId, entity.Properties["InstanceId"].StringValue);
            Assert.AreEqual(diagMessage.DiagnosticsInfo.SourceRegion, entity.Properties["SourceRegion"].StringValue);
            Assert.AreEqual(diagMessage.DiagnosticsInfo.SourceSiteName, entity.Properties["SourceSiteName"].StringValue);
            Assert.AreEqual(diagMessage.DiagnosticsInfo.Source, entity.Properties["Source"].StringValue);
        }

        private static Feedback GetFeedbackFromDb(DocumentationFeedbackDbDataService dataService, Guid correlationId, bool withComments = false)
        {
            Feedback feedback = null;

            int retries = 0;
            int sleepDelta = (int)configuration.RetryBackoffSpan.TotalSeconds;
            int sleepSeconds = sleepDelta;
            while (feedback == null || (string.IsNullOrEmpty(feedback.Comment) && withComments))
            {
                if (retries >= configuration.MaxRetries)
                {
                    // We couldn't find the processed feedback in the given time.
                    throw new TimeoutException("Could not found the feedback message in the database within the given time.");
                }
                else
                {
                    Thread.Sleep(sleepSeconds * 1000);

                    // Prepare for next retry.
                    sleepSeconds += sleepDelta;
                    retries++;
                }

                feedback = dataService.GetFeedback(correlationId);
            }

            return feedback;
        }

        private static CloudTable GetMessageDiagnosticsInfoTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.AzureWebjobStorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference(configuration.MessageDiagnosticsInfoTable);
        }

        private ArticleFeedbackMessage CreateFeedbackMessage()
        {
            var message = new ArticleFeedbackMessage
            {
                ArticleId = DummyArticleId,
                Culture = "en-us",
                ArticleLastModified = timestamp,
                Service = "test-service",
                FeedbackTimestamp = timestamp,
                Helpful = true,
                IpAddress = "127.0.0.1",

                // Dummy URLs. Those files do not exist.
                Url = @"https://powerbi.microsoft.com/es-es/documentation/powerbi-service-get-started/",
                GitHubSource = @"https://github.com/Azure/powerbi-content-pr/blob/master/articles/powerbi-service-get-started.md",

                // Dummy hash.
                Hash = "083c97ffc54d63740039a60246f23d827a11d2cf",
            };

            var diagMessage = (IDiagnosticInfo)message;
            diagMessage.MessageId = Guid.NewGuid();
            diagMessage.DiagnosticsInfo = new DiagnosticsInfo
            {
                SentTimestamp = timestamp,
                Source = "PowerBI.Jobs.FunctionalTests",
                SourceRegion = "West US",
                SourceSiteName = DummyArticleId,
                InstanceId = "0",
                DeploymentVersion = "0"
            };

            return message;
        }
    }
}
