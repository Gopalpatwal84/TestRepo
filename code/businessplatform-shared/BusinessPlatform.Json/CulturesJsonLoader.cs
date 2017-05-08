namespace BusinessPlatform.Json
{
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using BusinessPlatform.Entities;

    public class CulturesJsonLoader : JsonLoader<Culture>
    {
        public CulturesJsonLoader(IFileReader reader) : base(reader.ReadAllText("cultures.json"))
        {
        }
    }
}
