using System.Collections.Generic;
using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class ContinentsJsonLoader : JsonLoader<Continent>
    {
        private readonly CountriesJsonLoader countriesJsonLoader;

        public ContinentsJsonLoader(IFileReader fileReader, CountriesJsonLoader countriesJsonLoader)
            : base(fileReader.ReadAllText("continents.json"))
        {
            this.countriesJsonLoader = countriesJsonLoader;
        }

        protected override void Process(IEnumerable<Continent> data)
        {
            foreach (var continent in data)
            {
                continent.Countries = this.countriesJsonLoader.Data.Value.ResolveFromSlugs(continent.CountrySlugs);
            }
        }
    }
}
