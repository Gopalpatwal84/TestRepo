using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using PowerBI.Web.Models.Pages;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "CloudPreferenceCache")]
    public class PricingController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/pricing/")]
        public ActionResult Index()
        {
            var model = new ViewMetadataModel();

            return Request.QueryString["b"] == "1" ?
                View("pricing/index-b", model) :
                View("pricing/index", model);
        }
    }
}