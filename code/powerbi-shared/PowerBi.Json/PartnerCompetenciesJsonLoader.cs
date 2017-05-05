namespace PowerBI.Json
{
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using PowerBI.Entities;

    public class PartnerCompetenciesJsonLoader : JsonLoader<PartnerCompetency>
    {
        public PartnerCompetenciesJsonLoader(IFileReader reader) : base(reader.ReadAllText("partner-competencies.json"))
        {
        }
    }
}
