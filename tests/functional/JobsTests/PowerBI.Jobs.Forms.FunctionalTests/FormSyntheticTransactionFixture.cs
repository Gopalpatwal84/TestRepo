namespace PowerBI.Jobs.Forms.FunctionalTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Acom.Configuration.Services;
    using Acom.Configuration.Services.Specialized;
    using Acom.DocumentDb;
    using Autofac;
    using Data;
    using Marketo.API;
    using Marketo.API.Identity;
    using Marketo.API.Leads;
    using Mediation.Queues;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using PowerBI.Messages.Commands;
    using PowerBI.Messages.Models;

    [TestClass]
    public class FormSyntheticTransactionFixture
    {
        private static DateTime timestamp = DateTime.UtcNow;
        private static Configuration configuration;

        // Used for clean-up.
        private static List<string> entityToRemove;
        private static DynamicTableEntity tableEntityToRemove;
        private static IContainer container;
        private static DocumentDbConnectionInfo connectionInfo;

        [ClassInitialize]
        public static void LoadConfigurations(TestContext context)
        {
            entityToRemove = new List<string>();
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
                var documentDbClient = new DocumentDbClient(connectionInfo);
                foreach (var slug in entityToRemove)
                {
                    documentDbClient.DeleteAsync<User>(slug).Wait();
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
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSavedInTheDatabase_EmailOnly()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessageEmailOnly("save-email-only@funtional-test.com", "save-email-only");
            var diagMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();

            // Assert
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email);

            entityToRemove.Add(request.Slug);

            Assert.IsNotNull(request);
            Assert.AreEqual(message.FormType, request.Forms.First());
            Assert.AreEqual(message.Email, request.Email);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSavedInTheDatabase()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessage("save-db-only@functional-test.com", "save-db-only");
            var diagMessage = (IDiagnosticInfo)message;
            
            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();

            // Assert
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email);

            entityToRemove.Add(request.Slug);

            Assert.IsNotNull(request);
            Assert.AreEqual(message.Country, request.Country);
            Assert.AreEqual(message.FirstName, request.FirstName);
            Assert.AreEqual(message.JobTitle, request.JobTitle);
            Assert.AreEqual(message.LastName, request.LastName);
            Assert.AreEqual(message.FormType, request.Forms.First());
            Assert.AreEqual(message.Email, request.Email);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void QueueMessageWithExistentEmail_ShouldUpdateRequestEntry()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessage("update-db-only@functional-test.com", "update-db-only");
            var diagMessage = (IDiagnosticInfo)message;
            
            RegistrationRequestFormMessage newMessage = CreateMessage("update-db-only@functional-test.com", "update-db-only");
            newMessage.Country = "New Country";
            var diagNewMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email);

            mediator.Send(newMessage).Wait();

            // Assert
            var newRequest = GetRegistrationRequestFromDb(documentDbClient, message.Email, requireCountry: newMessage.Country);

            Assert.IsNotNull(newRequest);
            Assert.AreEqual(newMessage.Country, newRequest.Country);
            Assert.AreEqual(newMessage.FirstName, newRequest.FirstName);
            Assert.AreEqual(newMessage.JobTitle, newRequest.JobTitle);
            Assert.AreEqual(newMessage.LastName, newRequest.LastName);
            Assert.AreEqual(newMessage.FormType, newRequest.Forms.First());
            Assert.AreEqual(newMessage.Email, newRequest.Email);

            entityToRemove.Add(newRequest.Slug);

            AssertDiagnosticInfo(diagNewMessage);
        }
        
        [TestMethod]
        public void QueueMessageWithNotExistentType_ShouldSaveNewRequestEntry()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessage("same-email@functional-test.com", "form-type");
            var diagMessage = (IDiagnosticInfo)message;

            RegistrationRequestFormMessage newMessage = CreateMessage("same-email@functional-test.com", "new-form-type");
            var diagNewMessage = (IDiagnosticInfo)newMessage;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();

            // Wait until the first message is stored/read
            GetRegistrationRequestFromDb(documentDbClient, message.Email);
            mediator.Send(newMessage).Wait();

            // Assert
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email, waitForSecondForm: true);
            entityToRemove.Add(request.Slug);

            Assert.IsNotNull(request);
            Assert.AreEqual(message.Email, request.Email);
            Assert.AreEqual(message.FormType, request.Forms.First());
            Assert.AreEqual(newMessage.FormType, request.Forms.Skip(1).First());
            Assert.AreEqual(2, request.Forms.Count);

            AssertDiagnosticInfo(diagNewMessage);
        }

        [TestMethod]
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSentToMarketo()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessage("save-marketo@functional-test.com", "save-marketo", "functional-test-save");
            var diagMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();

            // Assert
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email);

            Assert.IsNotNull(request);
            Assert.AreEqual(message.Country, request.Country);
            Assert.AreEqual(message.FirstName, request.FirstName);
            Assert.AreEqual(message.JobTitle, request.JobTitle);
            Assert.AreEqual(message.LastName, request.LastName);
            Assert.AreEqual(message.FormType, request.Forms.First());
            Assert.AreEqual(message.Email, request.Email);

            AssertMarketoLead(message).Wait();

            entityToRemove.Add(request.Slug);

            AssertDiagnosticInfo(diagMessage);
        }

        [TestMethod]
        public void IfMessageIsAddedToTheQueue_ItShouldBeInstrumentedAndSentToMarketo_EmailOnly()
        {
            // Arrange
            RegistrationRequestFormMessage message = CreateMessageEmailOnly("save-marketo-email-only@functional-test.com", "save-marketo-email-only", "functional-test-save-email-only");
            var diagMessage = (IDiagnosticInfo)message;

            var mediator = new QueuedMediator(configuration.AzureWebjobStorageConnectionString, new AutofacResolver(container));

            var documentDbClient = new DocumentDbClient(connectionInfo);

            // Act
            mediator.Send(message).Wait();

            // Assert
            var request = GetRegistrationRequestFromDb(documentDbClient, message.Email);

            Assert.IsNotNull(request);
            Assert.AreEqual(message.FormType, request.Forms.First());
            Assert.AreEqual(message.Email, request.Email);

            AssertMarketoLeadEmailOnly(message).Wait();

            entityToRemove.Add(request.Slug);

            AssertDiagnosticInfo(diagMessage);
        }

        private static User GetRegistrationRequestFromDb(IDocumentDbClient documentDbClient, string slug, string requireCountry = null, bool waitForSecondForm = false)
        {
            User request = null;

            int retries = 0;
            int sleepDelta = (int)configuration.RetryBackoffSpan.TotalSeconds;
            int sleepSeconds = sleepDelta;
            while (request == null
                || (!string.IsNullOrEmpty(requireCountry) && !string.Equals(request.Country, requireCountry))
                || (waitForSecondForm && request.Forms.Count < 2))
            {
                if (retries >= configuration.MaxRetries)
                {
                    // We couldn't find the processed feedback in the given time.
                    throw new TimeoutException("Could not found the registration request message in the database within the given time.");
                }
                else
                {
                    Thread.Sleep(sleepSeconds * 1000);

                    // Prepare for next retry.
                    sleepSeconds += sleepDelta;
                    retries++;
                }

                request = documentDbClient.GetAsync<User>(slug).Result;
            }

            return request;
        }

        private static RegistrationRequestFormMessage CreateMessage(string email, string formType, string marketoResourceId = null)
        {
            var message = new RegistrationRequestFormMessage
            {
                FirstName = "Test FirstName",
                LastName = "Test LastName",
                JobTitle = "Test JobTitle",
                Email = email,
                Country = "UnitedStates",
                MarketoResourceId = marketoResourceId,
                FormType = formType,
                SubmittedOn = timestamp
            };

            var diagMessage = (IDiagnosticInfo)message;
            diagMessage.MessageId = Guid.NewGuid();
            diagMessage.DiagnosticsInfo = new DiagnosticsInfo
            {
                SentTimestamp = timestamp,
                Source = "PowerBI.Jobs.FunctionalTests",
                SourceRegion = "West US",
                SourceSiteName = string.Format("{0}-{1}", formType, email),
                InstanceId = "0",
                DeploymentVersion = "0"
            };

            return message;
        }

        private static RegistrationRequestFormMessage CreateMessageEmailOnly(string email, string formType, string marketoResourceId = null)
        {
            var message = new RegistrationRequestFormMessage
            {
                Email = email,
                MarketoResourceId = marketoResourceId,
                FormType = formType,
                SubmittedOn = timestamp
            };

            var diagMessage = (IDiagnosticInfo)message;
            diagMessage.MessageId = Guid.NewGuid();
            diagMessage.DiagnosticsInfo = new DiagnosticsInfo
            {
                SentTimestamp = timestamp,
                Source = "PowerBI.Jobs.FunctionalTests",
                SourceRegion = "West US",
                SourceSiteName = string.Format("{0}-{1}", formType, email),
                InstanceId = "0",
                DeploymentVersion = "0"
            };

            return message;
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

        private static async Task AssertMarketoLead(RegistrationRequestFormMessage formMessage)
        {
            var marketoConfiguration = new MarketoConfiguration();
            var identityService = new IdentityService(marketoConfiguration);
            identityService.Authenticate();
            var leadService = new LeadService(marketoConfiguration);
            var marketoResponse = await leadService.GetLeadsByFilterAsync("email", formMessage.Email, "firstName, lastName, company, phone, title, country");
            Assert.IsFalse(marketoResponse.HasErrors);
            Assert.IsTrue(marketoResponse.Results.Any());
            var lead = marketoResponse.Results.First();
            Assert.AreEqual(formMessage.MarketoLeadId, lead.Id);
            Assert.AreEqual(formMessage.FirstName, lead.FirstName);
            Assert.AreEqual(formMessage.LastName, lead.LastName);
            Assert.AreEqual(formMessage.Country, lead.Country);
            Assert.AreEqual(formMessage.JobTitle, lead.Title);
        }

        private static async Task AssertMarketoLeadEmailOnly(RegistrationRequestFormMessage formMessage)
        {
            var marketoConfiguration = new MarketoConfiguration();
            var identityService = new IdentityService(marketoConfiguration);
            identityService.Authenticate();
            var leadService = new LeadService(marketoConfiguration);
            var marketoResponse = await leadService.GetLeadsByFilterAsync("email", formMessage.Email);
            Assert.IsFalse(marketoResponse.HasErrors);
            Assert.IsTrue(marketoResponse.Results.Any());
            var lead = marketoResponse.Results.First();
            Assert.AreEqual(formMessage.MarketoLeadId, lead.Id);
        }
    }
}
