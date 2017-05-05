using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acom.Search.Client;
using Acom.Search.Client.Documents;
using Acom.Search.Client.Models;
using PowerBI.Search;
using PowerBI.Search.Documents;
using PowerBI.Web.Models;
using PowerBI.Web.Models.Pages.Search;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class SearchHelper
    {
        private readonly ISearchClient searchClient;
        
        public static readonly Dictionary<string, string> AvailableTypes = new Dictionary<string, string> {
            { "ArticleEntry", "documentation" },
            { "BlogEntry", "blog" },
            { "CommunityEntry", "community" },
            { "PartnerDirectoryProfileEntry", "partners" },
        };

        private static readonly string[] PartnerEntries = {
            nameof(PartnerDirectoryProfileEntry),
            nameof(PartnerShowcaseProfileEntry)
        };

        public SearchHelper(ISearchClient searchClient)
        {
            this.searchClient = searchClient;
        }
        
        public async Task<SearchIndexModel> Search<T>(string q, int page, int pageSize) where T : Entry
        {
            if (PartnerEntries.Contains(nameof(T)))
            {
                var response = await this.searchClient.CreateQuery<Entry>(q)
                .FilterByProperty(x => x.Type, PartnerEntries, SearchIndexOperator.IsEqual)
                .HighlightTitleSummaryContent()
                .HighlightFields(x => x.Url)
                .IncludeTotalResultCount()
                .Skip(pageSize * (page - 1))
                .Top(pageSize)
                .SearchAsync();

                return CreateSearchIndexModel(response, q, page, pageSize, nameof(T));
            }
            else
            {
                var response = await this.searchClient.CreateQuery<T>(q)
                    .HighlightTitleSummaryContent()
                    .HighlightFields(x => x.Url)
                    .IncludeTotalResultCount()
                    .Skip(pageSize * (page - 1))
                    .Top(pageSize)
                    .SearchAsync();

                return CreateSearchIndexModel(response, q, page, pageSize);
            }
        }

        private SearchIndexModel CreateSearchIndexModel<T>(AcomDocumentSearchResults<T> response, string q, int page, int pageSize, string type = null) where T : Entry
        {
            AvailableTypes.TryGetValue(typeof(T).Name, out type);

            return new SearchIndexModel
            {
                Term = q,
                Pagination = new Pagination(page, pageSize, response.Count),
                Type = type,
                Types = AvailableTypes,
                EntryResults = response.Results.OfType<AcomSearchResult<Entry>>()
                            .Where(x => x != null)
                            .ToArray(),
                ArticleEntryResults = response.Results.OfType<AcomSearchResult<ArticleEntry>>()
                            .Where(x => x != null)
                            .ToArray(),
                BlogEntryResults = response.Results.OfType<AcomSearchResult<BlogEntry>>()
                            .Where(x => x != null)
                            .ToArray(),
                CommunityEntryResults = response.Results.OfType<AcomSearchResult<CommunityEntry>>()
                            .Where(x => x != null)
                            .ToArray(),
                PartnerDirectoryEntryResults = response.Results.Select(x => x as AcomSearchResult<PartnerDirectoryProfileEntry>)
                            .Where(x => x != null)
                            .ToArray(),
                PartnerShowcaseEntryResults = response.Results.Select(x => x as AcomSearchResult<PartnerShowcaseProfileEntry>)
                            .Where(x => x != null)
                            .ToArray()
            };
        }
    }
}