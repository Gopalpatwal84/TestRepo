using Acom.Configuration.Settings;

namespace PowerBI.Jobs.SupportTopicsPump
{
    public class Configuration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public string SupportServicesUrl { get; set; }

        [ConfigurationSetting]
        public string ProductId { get; set; }
    }
}
