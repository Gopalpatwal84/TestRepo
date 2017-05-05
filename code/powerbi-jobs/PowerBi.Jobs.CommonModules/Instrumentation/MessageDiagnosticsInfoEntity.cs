namespace PowerBI.Jobs.CommonModules.Instrumentation
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;
    using PowerBI.Messages.Models;
    
    public class MessageDiagnosticsInfoEntity : TableEntity
    {
        private Guid messageId;

        private DateTime sentAt;

        public MessageDiagnosticsInfoEntity(DiagnosticsInfo diagnosticsInfo)
        {
            DeploymentVersion = diagnosticsInfo.DeploymentVersion;
            InstanceId = diagnosticsInfo.InstanceId;
            SentTimestamp = diagnosticsInfo.SentTimestamp;
            Source = diagnosticsInfo.Source;
            SourceRegion = diagnosticsInfo.SourceRegion;
            SourceSiteName = diagnosticsInfo.SourceSiteName;
        }

        public Guid MessageId
        {
            get
            {
                return messageId;
            }

            set
            {
                messageId = value;
                RowKey = value != null ? value.ToString() : null;
            }
        }

        public string MessageType { get; set; }

        public string QueueName { get; set; }

        public DateTime SentTimestamp
        {
            get
            {
                return sentAt;
            }

            set
            {
                sentAt = value;
                PartitionKey = value != null ? value.ToString("yyyy-MM-dd") : null;
            }
        }

        public DateTime ReceivedTimestamp { get; set; }

        public string Source { get; set; }

        public string SourceRegion { get; set; }

        public string SourceSiteName { get; set; }

        public string WebJobRegion { get; set; }

        public string WebJobSiteName { get; set; }

        public string InstanceId { get; set; }

        public string DeploymentVersion { get; set; }
    }
}
