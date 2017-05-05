using Acom.Search.Client;
using Acom.Search.Client.Documents;

namespace PowerBI.Search
{
    public static class QueryBuilderExtension
    {
        public static PowerBiQueryBuilder<T> CreateQuery<T>(this ISearchClient searchClient, string term = null) where T : Entry
        {
            return new PowerBiQueryBuilder<T>(searchClient, term);
        }
    }
}