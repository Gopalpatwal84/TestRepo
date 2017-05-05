using System.Collections.Generic;
using Acom.IO.Entities;

namespace PowerBI.Entities
{
    public class SupportProblemQuestions : IHaveSlug
    {
        public string Slug { get; set; }

        public string ProblemTypeId { get; set; }

        public IEnumerable<SupportQuestion> Questions { get; set; }
    }
}
