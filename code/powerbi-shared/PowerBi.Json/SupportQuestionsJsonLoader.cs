using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class SupportQuestionsJsonLoader : JsonLoader<SupportProblemQuestions>
    {
        public SupportQuestionsJsonLoader(IFileReader reader) : base(reader.ReadAllText("support-questions.json"))
        {
        }
    }
}
