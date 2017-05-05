using System.Web.Mvc;

namespace PowerBI.Web.Controllers
{
    public class AssetController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("asset/head/")]
        public ActionResult Head()
        {
            return View("asset/head");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("asset/header/")]
        public ActionResult Header()
        {
            ViewBag.HideForLithium = true;
            return PartialView("components/_Header");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("asset/footer/")]
        public ActionResult Footer()
        {
            return PartialView("components/_Footer");
        }
    }
}