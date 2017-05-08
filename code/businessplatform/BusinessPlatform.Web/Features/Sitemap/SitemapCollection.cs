namespace BusinessPlatform.Web.Features.Sitemap
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Caching;
    using Acom.IO;
    using Acom.Sitemap;
    using BusinessPlatform.Entities;

    public class SitemapCollection
    {
        private const string CacheKey = "sitemapcachekey";
        private readonly IRepository<PageMetadata> pageMetadataRepository;

        public SitemapCollection(IRepository<PageMetadata> pageMetadataRepository)
        {
            this.pageMetadataRepository = pageMetadataRepository;
        }

        public async Task<SitemapPageInfo[]> GetSitemapPageInfosAsync(Cache cache = null)
        {
            List<SitemapPageInfo> list = null;

            if (cache != null && cache[CacheKey] != null)
            {
                list = cache[CacheKey] as List<SitemapPageInfo>;
            }

            if (list == null)
            {
                list = new List<SitemapPageInfo>();

                var viewsMetadata = await this.pageMetadataRepository.GetAsync();
                list.AddRange(viewsMetadata.Select(x => new SitemapSimplePageInfo { RelativeUrl = x.Url }));

                if (cache != null)
                {
                    cache[CacheKey] = list;
                }
            }

            return list.ToArray();
        }
    }
}