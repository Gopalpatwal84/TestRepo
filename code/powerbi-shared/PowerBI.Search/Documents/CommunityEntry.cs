using Acom.Search.Client.Documents;
using Newtonsoft.Json;
using PowerBI.Entities;

namespace PowerBI.Search.Documents
{
    [ForEntity(typeof (CommunityMessage))]
    public class CommunityEntry : PowerBiEntry
    {
        [JsonConstructor]
        protected CommunityEntry()
        {
        }

        public CommunityEntry(string url, string slug) : base(url, slug)
        {
        }
    }
}
