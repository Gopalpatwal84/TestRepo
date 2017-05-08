namespace BusinessPlatform.Jobs.Web.Models
{
    public class VersionViewModel
    {
        public string SiteName { get; set; }

        public string Version { get; set; }

        public string InstanceId { get; set; }

        public string BranchName { get; set; }

        public string ServerName { get; set; }

        public string RegionName { get; set; }

        public string SlotName { get; set; }

        public string RedisGroup { get; set; }
    }
}