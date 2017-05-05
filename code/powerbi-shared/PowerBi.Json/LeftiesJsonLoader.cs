using System.Collections.Generic;
using System.Resources;
using Acom.IO.FileReaders;
using Acom.IO.Json;
using Newtonsoft.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class LeftiesJsonLoader : JsonLoader<Lefty>
    {
        private readonly IFileReader fileReader;

        public LeftiesJsonLoader(IFileReader fileReader) : base(string.Empty)
        {
            this.fileReader = fileReader;
        }

        protected override IEnumerable<Lefty> Deserialize(string json)
        {
            foreach (var lefty in this.fileReader.GetFileList("Lefties"))
            {
                var id = this.ExtractMiddleSegment(lefty);
                var fileText = this.fileReader.ReadAllText(lefty);
                yield return new Lefty
                {
                    Slug = id,
                    Maps = JsonConvert.DeserializeObject<Dictionary<string, LeftySection>>(fileText),
                    ResourceManager = new ResourceManager("PowerBI.Resources.Shared.Lefties." + id, typeof(Resources.Shared.Lefties.service).Assembly),
                };
            }
        }

        private string ExtractMiddleSegment(string fullPath)
        {
            return fullPath.Split('/')[1];
        }
    }
}
