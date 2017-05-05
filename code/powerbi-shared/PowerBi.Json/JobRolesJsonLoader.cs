using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class JobrolesJsonLoader : JsonLoader<Jobrole>
    {
        public JobrolesJsonLoader(IFileReader fileReader) : base(fileReader.ReadAllText("jobroles.json"))
        {
        }
    }
}
