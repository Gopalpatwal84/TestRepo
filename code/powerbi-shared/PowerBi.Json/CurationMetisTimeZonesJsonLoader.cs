using System;
using System.Collections.Generic;
using System.Linq;
using Acom.IO.FileReaders;
using Acom.IO.Json;
using PowerBI.Entities;

namespace PowerBI.Json
{
    public class CurationMetisTimeZonesJsonLoader : JsonLoader<Curation<TimeZoneInfo>>
    {
        public CurationMetisTimeZonesJsonLoader(IFileReader reader) : base(reader.ReadAllText("curation-metis-timezones.json"))
        {
        }

        protected override void Process(IEnumerable<Curation<TimeZoneInfo>> data)
        {
            var sysTimeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (var curation in data)
            {
                curation.Items = sysTimeZones.Where(t => curation.ItemSlugs.Any(s => s.Equals(t.Id, StringComparison.OrdinalIgnoreCase)));
            }
        }
    }
}
