using System.Collections.Generic;
using System.Linq;
using Acom.IO.Entities;
using Acom.IO.Json;
using Microsoft.Azure.Search.Models;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class FacetExtensions
    {
        public static T[] Resolve<T>(this IEnumerable<FacetResult> facetResults, IEnumerable<T> data) where T : class, IHaveSlug
        {
            return data.ResolveFromSlugs(facetResults.Select(x => x.AsValueFacetResult<string>().Value))
                          .Distinct()
                          .ToArray();
        }
    }
}