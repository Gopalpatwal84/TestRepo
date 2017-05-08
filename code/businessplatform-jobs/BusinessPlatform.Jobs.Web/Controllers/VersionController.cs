namespace BusinessPlatform.Jobs.Web.Controllers
{
    using System.Web.Mvc;
    using BusinessPlatform.Jobs.Web.Helpers;

    public class VersionController : Controller
    {
        [AllowAnonymous]
        [Route("version/")]
        public ActionResult Index()
        {
            var versionData = VersionHelper.GetVersionInfo();

            var json = CamelCaseSerializationHelper.SerializeObject(versionData);
            return this.Content(json, "application/json");
        }
    }
}