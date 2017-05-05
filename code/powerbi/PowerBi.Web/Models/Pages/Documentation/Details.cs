using System;
using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.Documentation
{
    public class Details : ViewMetadataModel
    {
        public Article Article { get; set; }

        public string Content { get; set; }

        public GithubAuthor[] Contributors { get; set; }

        public bool FeedbackEnabled { get; set; }

        public string GitHubProfileUrl { get; set; }

        public string GitHubAvatarUrl { get; set; }

        public DateTime LastModified { get; set; }

        public LeftNavigation LeftNavigation { get; set; }
    }
}