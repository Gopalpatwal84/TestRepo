using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.IO;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Web.Models.Pages.Departments;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class DepartmentsController : BaseController
    {
        private readonly IRepository<Department> departmentsRepository;

        public DepartmentsController(IRepository<Department> departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/departments/")]
        public async Task<ActionResult> Index()
        {
            var departments = await this.departmentsRepository.GetAsync();

            var model = new DepartmentsViewModel
            {
                Departments = departments.ToArray(),
            };

            return this.View("departments/index", model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/departments/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var departments = await this.departmentsRepository.GetAsync();
            var department = await this.departmentsRepository.GetAsync(slug);

            if (department == null) return this.NotFound();

            var model = new DepartmentDetailsViewModel
            {
                Departments = departments.ToArray(),
                Department = department,
            };

            return this.View("departments/_details", model);
        }
    }
}