using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CurrenciesJsonLoader : JsonLoader<Currency>
    {
        public CurrenciesJsonLoader(IFileReader reader) : base(reader.ReadAllText("currencies.json"))
        {
        }
    }
}
