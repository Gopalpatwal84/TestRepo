using System.Collections.Generic;
using PowerBI.Entities;
using PowerBI.Search.Documents;

namespace PowerBI.Web.Models.Pages.GuidedLearning
{
    public class Detail : ViewMetadataModel
    {
        public Dictionary<Course, ArticleEntry[]> CourseDictionary { get; set; }
        public Course SelectedCourse { get; set; }
        public Article Learning { get; set; }
        public Article NextLearning { get; set; }
        public string Content { get; set; }
    }
}