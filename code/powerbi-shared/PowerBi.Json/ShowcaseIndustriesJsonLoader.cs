using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class ShowcaseIndustriesJsonLoader : JsonLoader<ShowcaseIndustry>
    {
        public ShowcaseIndustriesJsonLoader(IFileReader reader) : base(reader.ReadAllText("showcase-industries.json"))
        {
        }
    }
}
