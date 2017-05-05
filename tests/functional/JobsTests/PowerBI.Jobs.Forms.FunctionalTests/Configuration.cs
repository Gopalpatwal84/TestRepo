using System;
using System.Configuration;
using System.Xml;

namespace PowerBI.Jobs.Forms.FunctionalTests
{
    public class Configuration
    {
        public Configuration()
        {
            this.MaxRetries = int.Parse(ConfigurationManager.AppSettings["RetryCount"]);
            this.RetryBackoffSpan = XmlConvert.ToTimeSpan(ConfigurationManager.AppSettings["RetryBackoffSpan"]);
            this.DoCleanUp = bool.Parse(ConfigurationManager.AppSettings["DoCleanUp"]);
            this.MessageDiagnosticsInfoTable = ConfigurationManager.AppSettings["MessageDiagnosticsInfoTable"];
            this.AzureWebjobStorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
        }

        public bool DoCleanUp { get; private set; }

        public int MaxRetries { get; private set; }

        public TimeSpan RetryBackoffSpan { get; private set; }

        public string MessageDiagnosticsInfoTable { get; private set; }

        public string AzureWebjobStorageConnectionString { get; private set; }
    }
}
