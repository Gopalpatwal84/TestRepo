using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CurationPartnerDirectoryJsonLoader : JsonLoader<Curation<Partner>>
    {
        public CurationPartnerDirectoryJsonLoader(IFileReader reader) : base(reader.ReadAllText("curation-partner-directory.json"))
        {
        }
    }
}
