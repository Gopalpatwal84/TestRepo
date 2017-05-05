using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using PowerBI.Web.Infrastructure.Helpers;
using PowerBI.Web.Models.Pages;
using PowerBI.Web.Models.Pages.DataJournalism;
using System.Linq;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "DataJournalismCache")]
    public class DatajournalismController : BaseController
    {

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/datajournalism/")]
        public ActionResult Index()
        {
            var model = new ViewMetadataModel();

            return View("datajournalism/index", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/datajournalism/courses/")]
        public ActionResult Courses()
        {
            var model = new ViewMetadataModel();

            return this.HttpContext.IsDataJournalismRegistrationComplete() ? View("datajournalism/courses/index", model) : (ActionResult)RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/datajournalism/courses/{slug}/")]
        public ActionResult Course(string slug)
        {
            var courseSlugs = new []
            {
                "visualization",
                "data-exploration",
                "truthful-visualizations",
                "choosing-graphics",
                "design-and-narrative"
            };         

            if (!courseSlugs.Contains(slug))
            {
                return this.View("_404", new ViewMetadataModel());
            }

            var registrationComplete = this.HttpContext.IsDataJournalismRegistrationComplete();

            if(!registrationComplete)
            {
                return (ActionResult)RedirectToAction("Index");
            }

            var viewModel = new Details
            {
                ActiveSlug = slug
            };

            return this.View($"datajournalism/courses/{slug}", viewModel);
        }
    }
}