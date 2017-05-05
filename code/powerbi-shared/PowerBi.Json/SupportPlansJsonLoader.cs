using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class SupportPlansJsonLoader : JsonLoader<SupportPlan>
    {
        public SupportPlansJsonLoader(IFileReader reader) : base(reader.ReadAllText("support-plans.json"))
        {
        }
    }
}
