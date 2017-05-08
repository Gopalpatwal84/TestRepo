namespace BusinessPlatform.Web.Controllers
{
    using System.Web.Mvc;
    using Acom.Configuration;

    public class VersionController : Controller
    {
        [AllowAnonymous]
        [Route("version/")]
        public JsonResult Index()
        {
            return new JsonResult
            {
                Data = new
                {
                    Version = KnownDeploymentVariables.DeploymentVersion,
                    SiteName = KnownAntaresVariables.SiteName,
                    InstanceId = KnownAntaresVariables.InstanceId,
                    BranchName = KnownDeploymentVariables.BranchName,
                    ServerName = this.Request.Params["SERVER_NAME"],
                    RegionName = KnownAntaresVariables.RegionName,
                    SlotName = KnownSlots.SlotName,
                    RedisGroup = KnownDeploymentVariables.DeploymentGroup,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                ContentType = "application/json"
            };
        }
    }
}