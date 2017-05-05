namespace PowerBI.Web.Infrastructure.Helpers
{
    using System.Collections.Generic;

    public class TermOfUseHelper
    {
        private static string defaultUrl = "https://www.microsoft.com/en-us/legal/intellectualproperty/copyright/default.aspx";
        private static IDictionary<string, string> localeSpecificUrls = new Dictionary<string, string>(){
                      {"de-de", "http://www.microsoft.com/de-de/rechtliche-hinweise/default.aspx"},
                      {"fr-fr", "http://www.microsoft.com/france/core/copyright.aspx"},
                      {"it-it", "http://www.microsoft.com/it-it/info/cpyright.aspx"},
                      {"ja-jp", "http://www.microsoft.com/ja-jp/mscorp/legal/misc/cpyright.aspx"},
                      {"nl-nl", "http://www.microsoft.com/netherlands/msnederland/gebruiksvoorwaarden.aspx"},
                      {"pl-pl", "http://www.microsoft.com/pl-pl/copyright/default.aspx"},
                      {"pt-br", "http://www.microsoft.com/brasil/misc/cpyright.htm"},
                      {"ru-ru", "http://news.microsoft.com/ru-ru/"},
                      {"en-au", "http://www.microsoft.com/australia/legal/terms-conditions.aspx"},
                      {"fr-ca", "http://www.microsoft.com/canada/fr/misc/cpyright.htm"}
        };

        public static string GetLocalizedUrl(string culture = null)
        {
            var currentCulture = string.IsNullOrEmpty(culture) ? CultureHelper.GetCurrentCulture(): culture;
            string localizedUrl;

            if (localeSpecificUrls.TryGetValue(currentCulture.ToLowerInvariant(), out localizedUrl))
            {
                return localizedUrl;
            }
            else
            {
                return defaultUrl;
            }
        }
    }
}