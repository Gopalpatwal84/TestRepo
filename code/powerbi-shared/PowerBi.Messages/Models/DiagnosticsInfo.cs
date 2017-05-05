namespace PowerBI.Messages.Models
{
    using System;

    public class DiagnosticsInfo
    {
        public DateTime SentTimestamp { get; set; }

        public string Source { get; set; }

        public string SourceRegion { get; set; }

        public string SourceSiteName { get; set; }

        public string InstanceId { get; set; }

        public string DeploymentVersion { get; set; }

        public string SlotName { get; set; }

        public string SourceIp { get; set; }
    }
}
