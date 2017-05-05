using Acom.Configuration.Settings;

namespace PowerBI.Web.Configuration
{
    public class PowerBIConfiguration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public bool ArticlesFeedbackEnabled { get; set; }

        [ConfigurationSetting]
        public string GitHubProfileUrl { get; set; }

        [ConfigurationSetting]
        public string GitHubAvatarUrl { get; set; }

        [ConfigurationSetting]
        public string ProductionDomain { get; set; }

        /// <summary>
        /// Gets or sets the URL of the UserVoice service status
        /// </summary>
        [ConfigurationSetting]
        public string ServiceStatusUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the Moked service status
        /// </summary>
        [ConfigurationSetting]
        public string TestServiceStatusUrl { get; set; }        
    }
}