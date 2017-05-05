using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Acom.IO;
using Acom.IO.FileReaders;
using PowerBI.Entities;

namespace PowerBI.Web.Controllers
{
    [RoutePrefix("patterns")]
    public class PatternsController : Controller
    {
        private readonly IRepository<Lefty> leftyRepository;
        private readonly IFileReader reader;

        public PatternsController(IRepository<Lefty> leftyRepository)
        {
            this.leftyRepository = leftyRepository;
            this.reader = new FileReader(HostingEnvironment.MapPath("~/Views/patterns"));
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("framework/{id?}")]
        public ActionResult Framework(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.RedirectToAction("/");
            }
            return this.View(string.Format("~/Views/patterns/framework/{0}.cshtml", id));
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("controls/{id?}")]
        public ActionResult Controls(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.RedirectToAction("/");
            }
            return this.View(string.Format("~/Views/patterns/controls/{0}.cshtml", id));
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("navigationleft")]
        public async Task<ActionResult> NavigationLeft()
        {
            var lefties = await this.leftyRepository.GetAsync();
            return this.View($"~/Views/patterns/navigationleft.cshtml", lefties);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{id?}")]
        public ActionResult Root(string id = null)
        {
            this.RouteData.Values.Add("culture", "en-us");
            if (string.IsNullOrEmpty(id))
            {
                return this.View("~/Views/patterns/index.cshtml", new MenuModel
                {
                    Controls = this.reader.GetFileList("controls").Select(x => Regex.Match(x, @"\\([a-z-]+)\.cshtml").Groups[1].Value).ToArray(),
                    Framework = this.reader.GetFileList("framework").Select(x => Regex.Match(x, @"\\([a-z-]+)\.cshtml").Groups[1].Value).ToArray(),
                    Misc = this.reader.GetFileList("").Select(x => Regex.Match(x, @"\\([a-z-]+)\.cshtml").Groups[1].Value).ToArray(),
                });
            }
            return this.View($"~/Views/patterns/{id}.cshtml");
        }
    }

    public class MenuModel
    {
        public string[] Controls { get; set; }
        public string[] Framework { get; set; }
        public string[] Misc { get; set; }
    }
}