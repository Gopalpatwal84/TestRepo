using System;
using Newtonsoft.Json;

namespace PowerBI.Jobs.CommunityPump.Models
{
    public class LithiumApiMessageData
    {
        public string Id { get; set; }

        [JsonProperty("view_href")]
        public string ViewHref { get; set; }

        public string Subject { get; set; }

        [JsonProperty("search_snippet")]
        public string SearchSnippet { get; set; }

        public string Body { get; set; }

        [JsonProperty("post_time")]
        public DateTime PostTime { get; set; }

        [JsonProperty("current_revision")]
        public CurrentRevision CurrentRevision { get; set; }
    }

    public class CurrentRevision
    {
        [JsonProperty("last_edit_time")]
        public DateTime LastEditTime { get; set; }
    }
}
