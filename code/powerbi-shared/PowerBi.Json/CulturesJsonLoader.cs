using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CulturesJsonLoader : JsonLoader<Culture>
    {
        public CulturesJsonLoader(IFileReader reader) : base(reader.ReadAllText("cultures.json"))
        {
        }
    }
}
