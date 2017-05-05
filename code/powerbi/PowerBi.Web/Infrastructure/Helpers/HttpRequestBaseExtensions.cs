namespace PowerBI.Web.Infrastructure.Helpers
{
    using System.Web;

    public static class HttpRequestBaseExtensions
    {
        public static string UserIPAddress(this HttpRequestBase incoming)
        {
            if (incoming.IsLocal)
            {
                return "127.0.0.1";
            }

            var ip = incoming.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? incoming.ServerVariables["REMOTE_ADDR"];

            return ip.Split(':')[0];
        }
    }
}