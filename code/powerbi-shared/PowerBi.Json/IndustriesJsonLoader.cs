using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class IndustriesJsonLoader : JsonLoader<Industry>
    {
        public IndustriesJsonLoader(IFileReader reader) : base(reader.ReadAllText("industries.json"))
        {
        }
    }
}
