using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CutomersJsonLoader : JsonLoader<Customer>
    {
        public CutomersJsonLoader(IFileReader reader) : base(reader.ReadAllText("customers.json"))
        {
        }
    }
}
