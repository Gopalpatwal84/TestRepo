using System;
using System.Collections.Generic;
using System.Linq;
using Acom.Configuration.Services;
using Acom.Search.Client;
using Acom.Search.Job.Host;
using Microsoft.Azure.Search.Models;

namespace PowerBI.Jobs.Search
{
    public class SearchIndexer : AcomSearchIndexerBase
    {
        public SearchIndexer(ServiceSettings settings, ISearchVersion version)
            : base(settings, version)
        {
        }

        internal static Index CreatIndexDefinition(string entryIndexName)
        {
            // When mapping to language analyzers from cultures use:
            //https://msdn.microsoft.com/en-us/library/azure/dn879793.aspx

            var contentOnlyTitleBoost = 55;

            var definition = new Index(
                entryIndexName,
                new List<Field>()
                    {
                        //Custom fields here
                        new Field("countries",      DataType.Collection(DataType.String)) { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = false, IsFacetable = true,  IsRetrievable = true },
                        new Field("cultures",       DataType.Collection(DataType.String)) { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = false, IsFacetable = false, IsRetrievable = true },
                        new Field("departments",    DataType.Collection(DataType.String)) { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = false, IsFacetable = true,  IsRetrievable = true },
                        new Field("externalId",     DataType.String)                      { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true,  IsFacetable = false, IsRetrievable = true },
                        new Field("industries",     DataType.Collection(DataType.String)) { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = false, IsFacetable = true,  IsRetrievable = true },
                        new Field("internalTags",   DataType.Collection(DataType.String)) { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = false, IsFacetable = true,  IsRetrievable = true },
                        new Field("titleCanonical", DataType.String)                      { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = true,  IsFacetable = false, IsRetrievable = true },
                    }
                    .Concat(DefaultFields())
                    .ToArray(),

                scoringProfiles: new[]
                    {
                        new ScoringProfile("default")
                            {
                                TextWeights = new TextWeights
                                    {
                                        Weights = new Dictionary<string, double>
                                            {
                                                { "title", contentOnlyTitleBoost },
                                            }.Concat(IterateNonEnCultures((culture, analyzer) => new KeyValuePair<string, double>(string.Format("title_{0}", culture), contentOnlyTitleBoost)))
                                            .ToDictionary(x => x.Key, x => x.Value)
                                    },
                            }

                    },
                defaultScoringProfile: "default",
                suggesters: new[]
                    {
                        new Suggester("default", SuggesterSearchMode.AnalyzingInfixMatching, new[] { "title" }.Concat(IterateCultures((culture, analyzer) => string.Format("title_{0}", culture))).ToList())
                    }
                );

            return definition;
        }

        protected override Index GetIndexDefinition(string entryIndexName)
        {
            return CreatIndexDefinition(entryIndexName);
        }
    }
}