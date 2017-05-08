namespace BusinessPlatform.Jobs.Web.Helpers
{
    using System.Web;
    using Acom.Configuration;
    using BusinessPlatform.Jobs.Web.Models;

    public class VersionHelper
    {
        // Read environment variables everytime we call this method so we don't cache between swaps
        public static VersionViewModel GetVersionInfo()
        {
            var version = new VersionViewModel
            {
                Version = KnownDeploymentVariables.DeploymentVersion,
                SiteName = KnownAntaresVariables.SiteName,
                InstanceId = KnownAntaresVariables.InstanceId,
                BranchName = KnownDeploymentVariables.BranchName,
                ServerName = HttpContext.Current.Request.Params["SERVER_NAME"],
                RegionName = KnownAntaresVariables.RegionName,
                SlotName = KnownSlots.SlotName,
                RedisGroup = KnownDeploymentVariables.DeploymentGroup,
            };

            return version;
        }
    }
}