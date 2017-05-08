namespace BusinessPlatform.Web.Infrastructure.Helpers
{
    using System;

    public static class CdnHelper
    {
        public static string GetCdnUrl(string url)
        {
            if (url.EndsWith(".gif"))
            {
                return string.Concat(url, $"?{DateTime.UtcNow.Ticks}");
            }

            return url;
        }
    }
}