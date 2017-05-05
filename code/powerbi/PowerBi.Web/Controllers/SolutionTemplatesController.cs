using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages.SolutionTemplates;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class SolutionTemplatesController : BaseController
    {
        private readonly IRepository<SolutionTemplate> solutionTemplateRepository;

        public SolutionTemplatesController(IRepository<SolutionTemplate> solutionTemplateRepository)
        {
            this.solutionTemplateRepository = solutionTemplateRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/solution-templates/")]
        public async Task<ActionResult> Index()
        {
            var templates = await this.solutionTemplateRepository.GetAsync();

            var model = new SolutionTemplatesViewModel
            {
               SolutionTemplates = templates.ToArray(),
            };

            return this.View("solution-templates/index", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/solution-templates/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var template = await this.solutionTemplateRepository.GetAsync(slug);
            if (template == null)
            {
                return this.NotFound();
            }

            var model = new SolutionTemplateDetailsViewModel
            {
                SolutionTemplate = template
            };

            return this.View("solution-templates/_details", model);
        }
    }
}