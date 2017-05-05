using System.Collections.Generic;

namespace PowerBI.Entities
{
    public class SupportQuestion
    {
        public string QuestionLockey { get; set; }

        public SupportQuestionType QuestionType { get; set; }

        public IEnumerable<string> PossibleAnswersLockeys { get; set; }

        public bool Required { get; set; }
    }
}
