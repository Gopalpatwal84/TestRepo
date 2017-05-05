using System.Collections.Generic;
using PowerBI.Entities;
using PowerBI.Search.Documents;

namespace PowerBI.Web.Models.Pages.GuidedLearning
{
    public class Index : ViewMetadataModel
    {
        public Dictionary<Course, ArticleEntry[]> CourseDictionary { get; set; }
    }
}