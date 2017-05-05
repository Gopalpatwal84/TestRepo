using Acom.Configuration.Settings;

namespace PowerBI.Support
{
    public class AuthConfiguration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public string Tenant { get; set; }

        [ConfigurationSetting]
        public string ClientId { get; set; }

        [ConfigurationSetting]
        public string AppKey { get; set; }

        [ConfigurationSetting]
        public string ResourceId { get; set; }

        public string AadInstance
        {
            get
            {
                return "https://login.windows.net/{0}";
            }
        }
    }
}
