namespace PowerBI.Web.Features.UserInfoCookie
{
    using System;
    using System.Web;
    using Acom.Configuration;
    using Acom.IO;
    using Newtonsoft.Json;

    public static class UserInfoCookieExtensions
    {
        private static readonly string UserInfoKey = "userInfo";

        public static void UpdateUserInfoCookie(this HttpRequest response, string email = null)
        {
            new HttpRequestWrapper(response).UpdateUserInfoCookie(email);
        }

        public static void UpdateUserInfoCookie(this HttpRequestBase request, string email = null)
        {
            var response = request.RequestContext.HttpContext.Response;
            var userCookie = request.Cookies[UserInfoKey] ?? new HttpCookie(UserInfoKey) { HttpOnly = false };

            if (string.IsNullOrEmpty(userCookie.Value) || !string.IsNullOrEmpty(email))
            {
                userCookie.Expires = Clock.UtcNow().AddYears(1).DateTime;

                if (string.IsNullOrEmpty(userCookie.Value))
                {
                    userCookie.Value = JsonConvert.SerializeObject(new
                    {
                        guid = Guid.NewGuid()
                    });
                }

                if (!string.IsNullOrEmpty(email))
                {
                    dynamic previous = null;
                    try
                    {
                        previous = JsonConvert.DeserializeObject(userCookie.Value);
                    }
                    catch
                    {
                        previous = new
                        {
                            guid = Guid.NewGuid()
                        };
                    }
                    userCookie.Value = JsonConvert.SerializeObject(new
                    {
                        previous.guid,
                        hashed = Hasher.NonCryptographic(email)
                    });
                }
                if (response.Cookies[UserInfoKey] != null)
                    response.Cookies.Set(userCookie);
                else
                    response.Cookies.Add(userCookie);
            }
        }

        public static string GetUserInfoCookieGuid(this HttpRequestBase request)
        {
            try
            {
                var userCookie = request.Cookies[UserInfoKey];
                if (userCookie != null)
                {
                    dynamic previous = JsonConvert.DeserializeObject(userCookie.Value);
                    return previous.guid;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}