namespace BusinessPlatform.Jobs.CommonModules.Instrumentation
{
    using System;
    using BusinessPlatform.Messages.Models;
    using Microsoft.WindowsAzure.Storage.Table;

    public class MessageDiagnosticsInfoEntity : TableEntity
    {
        private Guid messageId;

        private DateTime sentAt;

        public MessageDiagnosticsInfoEntity(DiagnosticsInfo diagnosticsInfo)
        {
            this.DeploymentVersion = diagnosticsInfo.DeploymentVersion;
            this.InstanceId = diagnosticsInfo.InstanceId;
            this.SentTimestamp = diagnosticsInfo.SentTimestamp;
            this.Source = diagnosticsInfo.Source;
            this.SourceRegion = diagnosticsInfo.SourceRegion;
            this.SourceSiteName = diagnosticsInfo.SourceSiteName;
        }

        public Guid MessageId
        {
            get
            {
                return this.messageId;
            }

            set
            {
                this.messageId = value;
                this.RowKey = value != null ? value.ToString() : null;
            }
        }

        public string MessageType { get; set; }

        public string QueueName { get; set; }

        public DateTime SentTimestamp
        {
            get
            {
                return this.sentAt;
            }

            set
            {
                this.sentAt = value;
                this.PartitionKey = value != null ? value.ToString("yyyy-MM-dd") : null;
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
