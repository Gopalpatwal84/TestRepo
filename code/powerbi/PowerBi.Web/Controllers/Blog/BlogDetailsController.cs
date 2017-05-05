using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using Acom.IO;
using Acom.IO.Entities;
using Acom.Mvc.Helpers;
using Acom.Mvc.Syndication;
using Acom.Search.Client;
using DevTrends.MvcDonutCaching;
using PowerBI.Entities.Blogs;
using PowerBI.Web.Configuration;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages.Blog;
using PowerBI.Web.Models.Parts;

namespace PowerBI.Web.Controllers.Blog
{
    [DonutOutputCache(CacheProfile = "BlogCache")]
    public class BlogDetailsController : BlogBaseController
    {
        private readonly PowerBIConfiguration _powerBiConfiguration;
        private readonly IRepository<BlogContent> _blogContentsRepository;

        public BlogDetailsController(
            PowerBIConfiguration powerBiConfiguration,
            IRepository<BlogPost> blogPostsRepository,
            IRepository<BlogCategory> blogTopicsRepository,
            IRepository<BlogContent> blogContentsRepository,
            ISearchClient searchClient)
            : base(blogPostsRepository, blogTopicsRepository, searchClient)
        {
            _powerBiConfiguration = powerBiConfiguration;
            _blogContentsRepository = blogContentsRepository;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/feed/")]
        public async Task<ActionResult> Feed()
        {
            var searchResults = await this.FromOrderedSet();

            var feed = new SyndicationFeed(
                Resources.Pages.Blog.Feed.META_PAGETITLE,
                Resources.Pages.Blog.Feed.META_DESCRIPTION,
                new Uri(Url.Action("Index", "BlogIndex", null, this.Request.Url?.Scheme)))
            {
                Language = "en-US",
                LastUpdatedTime = DateTime.UtcNow,
                AttributeExtensions =
                {
                    {
                        new XmlQualifiedName("slash", "http://www.w3.org/2000/xmlns/"),
                        "http://purl.org/rss/1.0/modules/slash/"
                    },
                    {
                        new XmlQualifiedName("content", "http://www.w3.org/2000/xmlns/"),
                        "http://purl.org/rss/1.0/modules/content/"
                    },
                    {
                        new XmlQualifiedName("wfw", "http://www.w3.org/2000/xmlns/"),
                        "http://wellformedweb.org/CommentAPI/"
                    },
                    {
                        new XmlQualifiedName("dc", "http://www.w3.org/2000/xmlns/"), "http://purl.org/dc/elements/1.1/"
                    }
                },
                Items = searchResults.Results.Select(x =>
                    new SyndicationItem(x.Title, x.Summary,
                        new Uri(Url.Action("Details", "BlogDetails", new { slug = x.Slug }, Request.Url.Scheme)))
                    {
                        PublishDate = x.Published.DateTime.ToPacificTime(),
                        Id = x.Slug,
                    }),
            };

            return new RssActionResult(feed);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        [Route("{culture=en-us}/blog/{slug}/")]
        public async Task<ActionResult> Details(string slug)
        {
            var blogPost = await BlogPostsRepository.GetAsync(slug);

            if (blogPost == null)
            {
                return this.NotFound();
            }

            var blogContent = await _blogContentsRepository.GetAsync(slug);

            blogPost.Published = blogPost.Published.DateTime.ToPacificTime();
            var viewModel = new Details
            {
                Post = blogPost,
                Content = blogContent?.Content,
                AllCategories = await this.GetCategories(),
                DisqusModel = this.GetDisqusModel(blogPost),
                SocialModel = new Social
                {
                    Title = blogPost.Title,
                    Description = blogPost.Summary
                }
            };

            return this.View("blog/_details", viewModel);
        }

        private Disqus GetDisqusModel(BlogPost blogPost)
        {
            // Shows comments only when using the default culture
            if (KnownCultures.CurrentNeutral == KnownCultures.DefaultLanguage)
            {
                var url = new Uri(
                    new Uri(_powerBiConfiguration.ProductionDomain),
                    string.Format(CultureInfo.InvariantCulture, "/{0}/blog/{1}/", KnownCultures.Default, blogPost.Slug));

                // Retrieves the Disqus threads from the blog forum
                return new Disqus
                {
                    Shortname = "thepowerbiblog",
                    Title = blogPost.Title,
                    Url = url.ToString(),
                    Identifier = string.Format(CultureInfo.InvariantCulture, "{0}-blog-{1}", KnownCultures.Default, blogPost.Slug)    
                };
            }

            return null;
        }
    }
}