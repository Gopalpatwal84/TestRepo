using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using Acom.IO;
using Acom.Mvc.Contracts;
using Acom.Sitemap;
using PowerBI.Entities;
using PowerBI.Entities.Blogs;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class SitemapCollection
    {
        private const string CacheKey = "sitemapcachekey";
        private readonly IRepository<Article> _articlesRepository;
        private readonly IRepository<BlogPost> _blogPostsRepository;
        private readonly IRepository<Department> _departmentsRepository;
        private readonly IRepository<Industry> _industriesRepository;
        private readonly IRepository<Integration> _integrationsRepository;
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<SolutionTemplate> _solutionTemplatesRepository;
        private readonly IRepository<PartnerShowcase> _partnerShowcaseRepository;

        public SitemapCollection(
            IRepository<Page> pageRepository,
            IRepository<Department> departmentsRepository,
            IRepository<BlogPost> blogPostsRepository,
            IRepository<Industry> industriesRepository,
            IRepository<Integration> integrationsRepository,
            IRepository<SolutionTemplate> solutionTemplatesRepository,
            IRepository<PartnerShowcase> partnerShowcaseRepository,
            IRepository<Article> articlesRepository)
        {
            _pageRepository = pageRepository;
            _departmentsRepository = departmentsRepository;
            _blogPostsRepository = blogPostsRepository;
            _industriesRepository = industriesRepository;
            _integrationsRepository = integrationsRepository;
            _solutionTemplatesRepository = solutionTemplatesRepository;
            _partnerShowcaseRepository = partnerShowcaseRepository;
            _articlesRepository = articlesRepository;
        }

        public async Task<SitemapPageInfo[]> GetSitemapPageInfosAsync(Cache cache = null)
        {
            List<SitemapPageInfo> list = null;

            if (cache?[CacheKey] != null)
            {
                list = cache[CacheKey] as List<SitemapPageInfo>;
            }

            if (list == null)
            {
                list = new List<SitemapPageInfo>();

                var pages = await _pageRepository.GetAsync();
                list.AddRange(pages.Select(page => new SitemapSimplePageInfo
                {
                    RelativeUrl = page.Url
                }));

                var blogPosts = await _blogPostsRepository.GetAsync();
                list.AddRange(blogPosts.Select(blogPost => new SitemapSimplePageInfo
                {
                    RelativeUrl = blogPost.Url
                }));

                var articles = await _articlesRepository.GetAsync();
                list.AddRange(articles.Select(article => new SitemapSimplePageInfo
                {
                    RelativeUrl = article.Url
                }));

                var departments = await _departmentsRepository.GetAsync();
                list.AddRange(departments.Select(department => new SitemapSimplePageInfo
                {
                    RelativeUrl = department.Url
                }));

                var industries = await _industriesRepository.GetAsync();
                list.AddRange(industries.Select(industry => new SitemapSimplePageInfo
                {
                    RelativeUrl = industry.Url
                }));

                var integrations = await _integrationsRepository.GetAsync();
                list.AddRange(integrations.Select(integration => new SitemapSimplePageInfo
                {
                    RelativeUrl = integration.Url
                }));

                var solutionTemplates = await _solutionTemplatesRepository.GetAsync();
                list.AddRange(solutionTemplates.Select(solutionTemplate => new SitemapSimplePageInfo
                {
                    RelativeUrl = solutionTemplate.Url
                }));

                var partnerShowcases = await _partnerShowcaseRepository.GetAsync();
                list.AddRange(partnerShowcases.Select(partner => new SitemapSimplePageInfo
                {
                    RelativeUrl = partner.Url,
                }));

                if (cache != null)
                {
                    cache[CacheKey] = list;
                }
            }

            return list.ToArray();
        }

        public async Task<SitemapPageInfo[]> GetSitemapPageInfosByAreaAsync(string areaName, Cache cache = null)
        {
            var sitemapPageInfos = await GetSitemapPageInfosAsync(cache);
            return sitemapPageInfos.Where(x => x.RelativeUrl.StartsWith(areaName)).ToArray();
        }
    }
}