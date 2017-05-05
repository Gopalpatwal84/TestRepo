using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;

namespace PowerBI.Web.Authorization
{
    public interface IAuthorizationAgent
    {
        Task<AuthorizeResult> AuthorizeAsync(AuthorizationCodeReceivedNotification context);
    }
}