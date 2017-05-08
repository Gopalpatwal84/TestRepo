namespace BusinessPlatform.Web.Configuration
{
    using Acom.Configuration;
    using Acom.Configuration.Settings;

    public class BusinessPlatformConfiguration : ConfigurationSettings
    {
        [ConfigurationSetting]
        public string CdnPrefix { get; set; }

        [ConfigurationSetting]
        public string CdnPrefixStagingSlot { get; set; }

        [ConfigurationSetting]
        public string ProductionDomain { get; set; }

        [ConfigurationSetting]
        public string LoadBalancedDomain { get; set; }

        public string GetCurrentCdnPrefix()
        {
            if (KnownSlots.IsStaging && !string.IsNullOrEmpty(this.CdnPrefixStagingSlot))
            {
                return this.CdnPrefixStagingSlot;
            }

            return this.CdnPrefix;
        }
    }
}