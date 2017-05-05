using System.Collections.Generic;
using Acom.Search.Client.Documents;
using Acom.Search.Client.Models;
using PowerBI.Search.Documents;

namespace PowerBI.Web.Models.Pages.Search
{
    public class SearchIndexModel : ViewMetadataModel
    {
        public string Term { get; set; }
        public Pagination Pagination { get; set; }
        public Dictionary<string, string> Types { get; set; }
        public string Type { get; set; }
        public AcomSearchResult<Entry>[] EntryResults { get; set; } = new AcomSearchResult<Entry>[0];
        public AcomSearchResult<BlogEntry>[] BlogEntryResults { get; set; } = new AcomSearchResult<BlogEntry>[0];
        public AcomSearchResult<ArticleEntry>[] ArticleEntryResults { get; set; } = new AcomSearchResult<ArticleEntry>[0];
        public AcomSearchResult<CommunityEntry>[] CommunityEntryResults { get; set; } = new AcomSearchResult<CommunityEntry>[0];
        public AcomSearchResult<PartnerDirectoryProfileEntry>[] PartnerDirectoryEntryResults { get; set; } = new AcomSearchResult<PartnerDirectoryProfileEntry>[0];
        public AcomSearchResult<PartnerShowcaseProfileEntry>[] PartnerShowcaseEntryResults { get; set; } = new AcomSearchResult<PartnerShowcaseProfileEntry>[0];

        public SearchResultModel<T> CreateResultModel<T>(AcomSearchResult<T> result) where T : Entry
        {
            return new SearchResultModel<T>(result);
        }
    }
}