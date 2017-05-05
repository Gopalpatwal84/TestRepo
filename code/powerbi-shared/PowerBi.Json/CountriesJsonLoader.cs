namespace PowerBI.Json
{
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using PowerBI.Entities;

    public class CountriesJsonLoader : JsonLoader<Country>
    {
        public CountriesJsonLoader(IFileReader reader) : base(reader.ReadAllText("countries.json"))
        {
        }
    }
}
