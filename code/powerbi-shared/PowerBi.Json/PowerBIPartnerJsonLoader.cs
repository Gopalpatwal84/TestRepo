using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class PowerBIPartnersJsonLoader : JsonLoader<PowerBIPartner>
    {
        public PowerBIPartnersJsonLoader(IFileReader reader) : base(reader.ReadAllText("powerbipartners.json"))
        {
        }
    }
}
