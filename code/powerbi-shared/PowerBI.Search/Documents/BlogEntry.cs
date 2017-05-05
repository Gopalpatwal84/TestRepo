using Acom.Search.Client.Documents;
using Newtonsoft.Json;
using PowerBI.Entities.Blogs;

namespace PowerBI.Search.Documents
{
    [ForEntity(typeof (BlogPost))]
    public class BlogEntry : PowerBiEntry, IBlogEntry
    {
        [JsonConstructor]
        protected BlogEntry() { }

        public BlogEntry(string url, string slug) : base(url, slug) { }

        public string PublisherSlug
        {
            get { return Get<string>(); }
            set { SetValue(value); }
        }

        public string PublisherPosition
        {
            get { return Get<string>(); }
            set { SetValue(value); }
        }

        public string PublisherProfileImage
        {
            get { return Get<string>(); }
            set { SetValue(value); }
        }
    }
}
