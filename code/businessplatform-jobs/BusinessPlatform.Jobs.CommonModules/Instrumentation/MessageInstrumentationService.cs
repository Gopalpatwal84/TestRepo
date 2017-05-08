namespace BusinessPlatform.Jobs.CommonModules.Instrumentation
{
    using System;
    using System.Linq;
    using Acom.Configuration;
    using Acom.Configuration.Services;
    using BusinessPlatform.Messages.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class MessageInstrumentationService : IMessageInstrumentationService
    {
        private readonly string webJobRegionName = KnownAntaresVariables.RegionName;

        private readonly string webJobSiteName = KnownAntaresVariables.SiteName;

        private readonly string storageConnectionString;

        public MessageInstrumentationService(ServiceSettings settings)
        {
            this.storageConnectionString = settings.ServiceGroups.First(x => x.Name == "BusinessPlatform-jobs")
                .StorageConnections
                .Default()
                .ConnectionString;
        }

        public void SaveOrUpdateInstrumentationInfo(IDiagnosticInfo message, DateTime receivedAt, string queueName)
        {
            var entity = this.GenerateDiagnosticsEntity(message, receivedAt, queueName);

            // If a message was sent using an older version the DiagnosticsInfo might not be available.
            if (entity == null)
            {
                return;
            }

            var table = this.GetInstrumentationTable();

            // By default any storage operations in 2.0 and above use a RetryExponential policy.
            var insertOrReplace = TableOperation.InsertOrReplace(entity);
            table.Execute(insertOrReplace);
        }

        private CloudTable GetInstrumentationTable()
        {
            var tableWithSlotname = KnownSlots.IsProduction ? string.Empty : KnownSlots.SlotName;

            var storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("messagediagnosticsinfo" + tableWithSlotname);

            table.CreateIfNotExists();
            return table;
        }

        private MessageDiagnosticsInfoEntity GenerateDiagnosticsEntity(IDiagnosticInfo message, DateTime receivedAt, string queueName)
        {
            if (message.DiagnosticsInfo == null)
            {
                return null;
            }

            var entity = new MessageDiagnosticsInfoEntity(message.DiagnosticsInfo)
            {
                MessageId = message.MessageId,
                MessageType = message.GetType().Name,
                ReceivedTimestamp = receivedAt,
                QueueName = queueName,
                WebJobRegion = this.webJobRegionName,
                WebJobSiteName = this.webJobSiteName
            };

            return entity;
        }
    }
}
