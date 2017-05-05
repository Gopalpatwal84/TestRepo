using Acom.Search.Client;
using Acom.Search.Client.Documents;

namespace PowerBI.Search
{
    public class PowerBiQueryBuilder<T> : QueryBuilder<PowerBiQueryBuilder<T>, T> where T : Entry
    {
        public PowerBiQueryBuilder(ISearchClient client, string term) : base(client, term)
        {
        }
    }
}