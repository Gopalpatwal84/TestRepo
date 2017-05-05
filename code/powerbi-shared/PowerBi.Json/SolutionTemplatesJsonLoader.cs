using System.Collections.Generic;
using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;
using System.Linq;

namespace PowerBI.Json
{
    public class SolutionTemplatesJsonLoader : JsonLoader<SolutionTemplate>
    {
        private readonly PowerBIPartnersJsonLoader partnersData;

        public SolutionTemplatesJsonLoader(IFileReader reader, PowerBIPartnersJsonLoader partnersData) : base(reader.ReadAllText("solution-templates.json"))
        {
            this.partnersData = partnersData;
        }

        protected override void Process(IEnumerable<SolutionTemplate> data)
        {
            foreach (var solution in data)
            {
                solution.FeaturedPartners = partnersData.Data.Value.ResolveFromSlugs(solution.FeaturedPartnerSlugs).ToArray();
                solution.EtlPartners = partnersData.Data.Value.ResolveFromSlugs(solution.EtlPartnerSlugs).ToArray();
            }
        }
    }
}
