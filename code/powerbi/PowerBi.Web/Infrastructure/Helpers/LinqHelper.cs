using System.Collections.Generic;
using System.Linq;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class LinqHelper
    {
        /// <remarks>
        /// returns N number groups of specificed size
        /// http://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq/6362642#6362642
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> items, int chunksize)
        {
            while (items.Any())
            {
                yield return items.Take(chunksize);
                items = items.Skip(chunksize);
            }
        }
    }
}