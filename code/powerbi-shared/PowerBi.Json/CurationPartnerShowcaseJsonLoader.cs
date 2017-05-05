using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CurationPartnerShowcaseJsonLoader : JsonLoader<Curation<PartnerShowcase>>
    {
        public CurationPartnerShowcaseJsonLoader(IFileReader reader) : base(reader.ReadAllText("curation-partner-showcase.json"))
        {
        }
    }
}
