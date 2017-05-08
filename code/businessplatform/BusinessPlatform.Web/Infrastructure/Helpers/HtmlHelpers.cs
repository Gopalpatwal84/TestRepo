namespace BusinessPlatform.Web.Infrastructure.Helpers
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlHelpers
    {
        public static IHtmlString LocalizeUrl(this HtmlHelper helper, string url)
        {
            return MvcHtmlString.Create(new CultureHelper().LocalizeUrl(url));
        }

        public static IHtmlString FullyLocalizeUrl(this HtmlHelper helper, string url)
        {
            if (url.StartsWith("http")) throw new Exception($"Only use {nameof(FullyLocalizeUrl)} with relative urls");

            // BusinessPlatform is reverse proxied so we can't use the request to find the url
            return MvcHtmlString.Create($"https://BusinessPlatform.microsoft.com{new CultureHelper().LocalizeUrl(url)}");
        }
    }
}