namespace PowerBI.Json
{
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using PowerBI.Entities;

    public class IntegrationsJsonLoader : JsonLoader<Integration>
    {
        public IntegrationsJsonLoader(IFileReader reader) : base(reader.ReadAllText("integrations.json"))
        {
        }
    }
}
