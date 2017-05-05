namespace PowerBI.Jobs.Forms.SignupRequests.FunctionalTests
{
    using System;
    using System.Configuration;
    using System.Xml;
    using Acom.Configuration.Services;
    using Acom.Configuration.Services.Specialized;

    public class Configuration
    {
        public Configuration()
        {
            this.DoCleanUp = bool.Parse(ConfigurationManager.AppSettings["DoCleanUp"]);
            this.MaxRetries = int.Parse(ConfigurationManager.AppSettings["RetryCount"]);
            this.RetryBackoffSpan = XmlConvert.ToTimeSpan(ConfigurationManager.AppSettings["RetryBackoffSpan"]);

            this.MessageDiagnosticsInfoTable = ConfigurationManager.AppSettings["MessageDiagnosticsInfoTable"];
            this.AzureWebjobStorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
        }

        public bool DoCleanUp { get; private set; }

        public int MaxRetries { get; private set; }

        public TimeSpan RetryBackoffSpan { get; private set; }

        public string MessageDiagnosticsInfoTable { get; private set; }

        public string AzureWebjobStorageConnectionString { get; private set; }

        public DocumentDbConnectionInfo DocumentDbConnectionInfo { get; internal set; }
    }
}
