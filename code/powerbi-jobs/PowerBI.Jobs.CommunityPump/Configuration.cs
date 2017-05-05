using Acom.Configuration.Settings;

namespace PowerBI.Jobs.CommunityPump
{
    public class Configuration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public string LithiumApiEndpoint { get; set; }

        [ConfigurationSetting]
        public string LithiumApiTenantId { get; set; }

        [ConfigurationSetting]
        public string LithiumApiClientId { get; set; }
    }
}
