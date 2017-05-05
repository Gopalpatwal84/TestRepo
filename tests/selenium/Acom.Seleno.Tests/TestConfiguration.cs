using Acom.Configuration.Settings;

namespace Acom.Selenium.Tests
{

    public class TestConfiguration : ConfigurationSettings
    {
        private static TestConfiguration instance;
        public static TestConfiguration Instance
        {
            get
            {
                return instance ?? (instance = new TestConfiguration());
            }
        }

        [ConfigurationSetting]
        public bool EnableAppliTools { get; set; }

        [ConfigurationSetting]
        public string AppliToolsKey { get; set; }

        [ConfigurationSetting]
        public string RootBranchName { get; set; }

        [ConfigurationSetting]
        public string Project { get; set; }

        [ConfigurationSetting]
        public string DefaultBatchName { get; set; }

        [ConfigurationSetting]
        public string EnvironmentUrl { get; set; }

        [ConfigurationSetting]
        public string BrowserStackUser { get; set; }

        [ConfigurationSetting]
        public string BrowserStackKey { get; set; }

        [ConfigurationSetting]
        public string SeleniumServerHubUrl { get; set; }

        [ConfigurationSetting]
        public string Browser { get; set; }

        [ConfigurationSetting]
        public string Os { get; set; }

        [ConfigurationSetting]
        public int ScreenX { get; set; }

        [ConfigurationSetting]
        public int ScreenY { get; set; }
    }
}
