using System.Web;
using System.Web.Mvc;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString LocalizeUrl(this HtmlHelper helper, string url, bool isFullyQualified = false)
        {
            return MvcHtmlString.Create(new CultureHelper().LocalizeUrl(url, isFullyQualified: isFullyQualified));
        }
    }
}