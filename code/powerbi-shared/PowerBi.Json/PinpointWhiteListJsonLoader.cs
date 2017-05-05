using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class PinpointWhiteListJsonLoader : JsonLoader<PinpointPartner>
    {
        public PinpointWhiteListJsonLoader(IFileReader reader) : base(reader.ReadAllText("pinpoint-whitelist.json"))
        {
        }
    }
}
