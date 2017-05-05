using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class SupportRegionSkusJsonLoader : JsonLoader<SupportRegionSku>
    {
        public SupportRegionSkusJsonLoader(IFileReader reader) : base(reader.ReadAllText("support-region-skus.json"))
        {
        }
    }
}
