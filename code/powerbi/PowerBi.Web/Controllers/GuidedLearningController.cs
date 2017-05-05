using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.Articles;
using Acom.IO;
using Acom.IO.Entities;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities;
using PowerBI.Search;
using PowerBI.Search.Documents;
using PowerBI.Web.Models.Pages.GuidedLearning;
using Serilog;

namespace PowerBI.Web.Controllers
{
    [DonutOutputCache(CacheProfile = "SiteCache")]
    public class GuidedLearningController : BaseController
    {
        private readonly IRepository<Course> _coursesRepository;
        private readonly ISearchClient _searchClient;
        private readonly IRepository<Article> _articlesRepository;
        private readonly IRepository<ArticleContent> _articleContentsRepository;

        public GuidedLearningController(
            IRepository<Course> coursesRepository,
            ISearchClient searchClient,
            IRepository<Article> articlesRepository,
            IRepository<ArticleContent> articleContentsRepository)
        {
            _coursesRepository = coursesRepository;
            _searchClient = searchClient;
            _articlesRepository = articlesRepository;
            _articleContentsRepository = articleContentsRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/guided-learning/")]
        public async Task<ActionResult> Index()
        {
            var viewModel = new Index
            {
                CourseDictionary = await BuildCourseDictionary(),
            };

            return View("guided-learning/index", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/guided-learning/{slug}/")]
        public async Task<ActionResult> Detail(string slug)
        {
            var learning = await _articlesRepository.GetAsync(slug);
            if (learning == null)
            {
                return NotFound();
            }

            learning = learning.OverrideArticleCulture(CultureHelperBase.GetCurrentCulture());
            var courseDictionary = await BuildCourseDictionary();
            var selectedCourse = courseDictionary?.FirstOrDefault(keyvalue => keyvalue.Value.Any(article => article.Slug == slug));
            var articleContent =
                await _articleContentsRepository.GetAsync($"{CultureHelperBase.GetCurrentCulture()}-{slug}") ??
                await _articleContentsRepository.GetAsync($"{CultureHelperBase.GetDefaultCulture()}-{slug}");

            var viewModel = new Detail
            {
                Learning = learning,
                CourseDictionary = courseDictionary,
                SelectedCourse = selectedCourse?.Key,
                Content = articleContent?.Content,
            };

            var nextLearningSlug = await GetNextLearningSlugOrDefault(learning);
            if (nextLearningSlug != null)
            {
                var nextLearning = await _articlesRepository.GetAsync(nextLearningSlug);
                viewModel.NextLearning = nextLearning?.OverrideArticleCulture(CultureHelperBase.GetCurrentCulture());
            }

            foreach (var tag in learning.Tags)
            {
                viewModel.AdditionalMetaTags.Add(tag.Key, tag.Value);
            }

            return View("guided-learning/_detail", viewModel);
        }

        internal async Task<string> GetNextLearningSlugOrDefault(Article learning)
        {
            var allCourses = (await _coursesRepository.GetAsync()).ToList();
            var currentCourse = allCourses.FirstOrDefault(x => x?.ArticleSlugs?.Contains(learning?.Slug) == true);
            var currentCourseLearnings = currentCourse?.ArticleSlugs?.ToList();

            return
                currentCourseLearnings
                    ?.ElementAtOrDefault(currentCourseLearnings.FindIndex(x => x == learning?.Slug) + 1) ??
                allCourses
                    .ElementAtOrDefault(allCourses.FindIndex(x => x?.Slug == currentCourse?.Slug) + 1)
                    ?.ArticleSlugs
                    ?.FirstOrDefault();
        }

        private async Task<Dictionary<Course, ArticleEntry[]>> BuildCourseDictionary()
        {
            var courseDictionary = new Dictionary<Course, ArticleEntry[]>();

            try
            {
                var courses = await _coursesRepository.GetAsync();
                var allLearnings = (await _searchClient.CreateQuery<ArticleEntry>()
                    .AnyTermInCollection(x => x.InternalTags, PowerBiEntry.GuidedLearningTag)
                    .SearchAllAsync()).ToArray();

                foreach (var course in courses)
                {
                    courseDictionary[course] =
                        course.ArticleSlugs
                        .Select(articleSlug => allLearnings.FirstOrDefault(x => string.Equals(articleSlug, x.Slug, StringComparison.InvariantCultureIgnoreCase)))
                        .Where(x => x != null)
                        .ToArray();
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Guided Learning failure building course dictionary");
            }

            return courseDictionary;
        }
    }
}