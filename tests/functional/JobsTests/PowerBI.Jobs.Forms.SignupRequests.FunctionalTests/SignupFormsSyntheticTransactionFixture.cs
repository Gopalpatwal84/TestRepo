namespace PowerBI.Jobs.Forms.SignupRequests.FunctionalTests
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;
    using Acom.Configuration.Services;
    using Acom.Configuration.Services.Specialized;
    using Acom.DocumentDb;
    using Autofac;
    using Data;
    using Mediation.Queues;
    using Messages.Commands;
    using Messages.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    [TestClass]
    public class SignupFormsSyntheticTransactionFixture
    {
        private static readonly DateTime Timestamp = DateTime.UtcNow;
        private static readonly string StorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
        private static Configuration configuration;

        // Used for clean-up.
        private static string entityToRemoveSlug;
        private static DynamicTableEntity tableEntityToRemove;
        private static IContainer container;
        private static DocumentDbConnectionInfo connectionInfo;

        [ClassInitialize]
        public static void LoadConfigurations(TestContext context)
        {
            entityToRemoveSlug = null;
            tableEntityToRemove = null;
            configuration = new Configuration();

            var settings = new ServiceSettings();
            connectionInfo = settings.ServiceGroups.Default.DocumentDbConnections.Default();

            var builder = new ContainerBuilder();
            container = builder.Build();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            if (configuration.DoCleanUp)
            {
                if (!string.IsNullOrEmpty(entityToRemoveSlug))
                {
                    var documentDbClient = new DocumentDbClient(connectionInfo);
                    documentDbClient.DeleteAsync<User>(entityToRemoveSlug).Wait();
                }
            }
        }

        [TestMethod]
        public async Task IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSavedInTheDatabase_AndUpdatedWithNewEvent()
        {
            // Arrange
            var documentDbClient = new DocumentDbClient(connectionInfo);
            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var message = this.CreateSignupRequestFormMessage();
            var diagMessage = (IDiagnosticInfo)message;

            // Holds the database entity's Guid to remove it during clean-up.
            entityToRemoveSlug = message.Email;

            // Act
            await mediator.Send(message);

            // Assert
            var signupRequest = await GetSignupRequestFromDb(documentDbClient, message.Email);

            Assert.AreEqual(message.Email, signupRequest.Email);
            Assert.AreEqual(message.IpAddress, signupRequest.IpAddress);

            AssertDiagnosticInfo(diagMessage);

            // Arrange second message
            var message2 = this.CreateSignupRequestFormMessage();

            // Change IpAddress to test updates
            message2.IpAddress = "128.0.0.2";
            var diagMessage2 = (IDiagnosticInfo)message2;

            // Act
            await mediator.Send(message2);

            // Assert
            var signupRequest2 = await GetSignupRequestFromDb(documentDbClient, message2.Email, waitForIpAddress: message2.IpAddress);

            Assert.AreEqual(message2.Email, signupRequest2.Email);
            Assert.AreEqual(message2.IpAddress, signupRequest2.IpAddress);
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

        private static CloudTable GetMessageDiagnosticsInfoTable()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(configuration.AzureWebjobStorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference(configuration.MessageDiagnosticsInfoTable);
        }

        private static async Task<User> GetSignupRequestFromDb(DocumentDbClient documentDbClient, string slug, string waitForIpAddress = null)
        {
            User signupRequest = null;

            int retries = 0;
            int sleepDelta = (int)configuration.RetryBackoffSpan.TotalSeconds;
            int sleepSeconds = sleepDelta;
            while (signupRequest == null || (!string.IsNullOrEmpty(waitForIpAddress) && !string.Equals(signupRequest.IpAddress, waitForIpAddress)))
            {
                if (retries >= configuration.MaxRetries)
                {
                    // We couldn't find the processed signup in the given time.
                    throw new TimeoutException("Could not found the signup message in the database within the given time.");
                }
                else
                {
                    Thread.Sleep(sleepSeconds * 1000);

                    // Prepare for next retry.
                    sleepSeconds += sleepDelta;
                    retries++;
                }

                signupRequest = await documentDbClient.GetAsync<User>(slug);
            }

            return signupRequest;
        }

        private SignUpRequestFormMessage CreateSignupRequestFormMessage()
        {
            var message = new SignUpRequestFormMessage
            {
                Email = "synthetic-signup-test@contoso.com",
                Date = DateTime.UtcNow,
                FormType = "synthetic-test",
                IpAddress = "128.0.0.1",
            };

            var diagMessage = (IDiagnosticInfo)message;
            diagMessage.MessageId = Guid.NewGuid();
            diagMessage.DiagnosticsInfo = new DiagnosticsInfo
            {
                SentTimestamp = Timestamp,
                Source = "Acom.Jobs.FunctionalTests",
                SourceRegion = "West US",
                SourceSiteName = "ACOM",
                InstanceId = "0",
                DeploymentVersion = "0"
            };

            return message;
        }
    }
}
