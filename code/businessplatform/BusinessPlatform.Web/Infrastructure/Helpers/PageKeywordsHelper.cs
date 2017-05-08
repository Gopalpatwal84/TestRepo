namespace BusinessPlatform.Web.Infrastructure.Helpers
{
    using System.Collections.Generic;
    using System.Text;

    public class PageKeywordsHelper
    {
        public const string KeywordsSeparator = ",";

        public static string EscapeKeywords(IEnumerable<string> keywords)
        {
            var keywordsBuilder = new StringBuilder();
            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    keywordsBuilder.Append(string.Concat(keyword.Replace(KeywordsSeparator, string.Empty), KeywordsSeparator));
                }
            }

            var keywordsString = keywordsBuilder.ToString();
            if (keywordsString.EndsWith(KeywordsSeparator))
            {
                keywordsString = keywordsString.Remove(keywordsString.Length - 1);
            }

            return keywordsString;
        }
    }
}