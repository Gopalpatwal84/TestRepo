using Acom.Search.Client.Documents;
using Acom.Search.Client.Models;

namespace PowerBI.Web.Models.Pages.Search
{
    public class SearchResultModel<T> where T : Entry
    {
        public SearchResultModel(AcomSearchResult<T> result)
        {
            this.Result = result;
        }

        public AcomSearchResult<T> Result { get; set; } 
    }
}