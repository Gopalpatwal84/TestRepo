using System.Web.Mvc;

namespace PowerBI.Jobs.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Json("homepage", JsonRequestBehavior.AllowGet);
        }
    }
}