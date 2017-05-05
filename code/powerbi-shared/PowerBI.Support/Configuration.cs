using Acom.Configuration.Settings;

namespace PowerBI.Support
{
    public class Configuration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public string MetisAPIUrl { get; set; }
    }
}
