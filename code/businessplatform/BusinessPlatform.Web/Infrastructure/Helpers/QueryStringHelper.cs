namespace BusinessPlatform.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The QueryStringHelper Helper class which is used to process all query string parameters
    /// </summary>
    public static class QueryStringHelper
    {
        public static string FormatQueryString(this NameValueCollection collection, params string[] ignoreKeys)
        {
            if (collection.AllKeys.Any() == false)
            {
                return string.Empty;
            }

            return collection.AllKeys
                .Where(x => ignoreKeys.Any(y => string.Equals(y, x, StringComparison.OrdinalIgnoreCase)) == false)
                .Aggregate("?", (agg, key) => string.Format("{0}{1}={2}&", agg, key, HttpUtility.UrlEncode(collection[key]))).TrimEnd('&', '?');
        }
    }
}