namespace BusinessPlatform.Json
{
    using System.Collections.Generic;
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using Entities;
    using Newtonsoft.Json;

    public class PageMetadataJsonLoader : JsonLoader<PageMetadata>
    {
        public PageMetadataJsonLoader(
            FileReader fileReader) // import the FileReader, not the IFileReader -- not doing the embedded resource thing for this file
            : base(fileReader.ReadAllText("views-metadata.json"))
        {
        }

        protected override IEnumerable<PageMetadata> Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<PagesMetadataModel>(json).Metadata;
        }

        protected override void Process(IEnumerable<PageMetadata> data)
        {
            foreach (var page in data)
            {
                page.SetMetaValuesFromPrimaryResx();
            }
        }
    }

    internal class PagesMetadataModel
    {
        public string Comment { get; set; }

        public IEnumerable<PageMetadata> Metadata { get; set; }
    }
}
