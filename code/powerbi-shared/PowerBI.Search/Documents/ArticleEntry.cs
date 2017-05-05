using System;
using System.Collections.Generic;
using Acom.Search.Client.Documents;
using Newtonsoft.Json;
using PowerBI.Entities;

namespace PowerBI.Search.Documents
{
    [ForEntity(typeof (Article))]
    public class ArticleEntry : PowerBiEntry, IArticleEntry
    {
        [JsonConstructor]
        protected ArticleEntry()
        {
        }

        public ArticleEntry(string url, string slug) : base(url, slug)
        {
        }

        public IEnumerable<string> Authors
        {
            get { return this.Get<IEnumerable<string>>(); }
            set { this.SetValue(value); }
        }

        [JsonIgnore]
        public ArticleEntryExtraData ExtraData
        {
            get
            {
                return GetExtraData<ArticleEntryExtraData>();
            }
            set
            {
                SetExtraData(value);
            }
        }

        public class ArticleEntryExtraData
        {
            public TimeSpan? Duration { get; set; }
        }
    }
}
