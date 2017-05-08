namespace BusinessPlatform.Jobs.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.Json("homepage", JsonRequestBehavior.AllowGet);
        }
    }
}