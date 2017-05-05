using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;

namespace PowerBI.Web.Authorization
{
    public class AuthorizationManager
    {
        // order is important, each agent can allow or skip the execution of next agents
        private static List<IAuthorizationAgent> agents = new List<IAuthorizationAgent>
        {
            new SupportProAuthorizationAgent(),
            new TenantAuthorizationAgent()
        };

        public static async Task AuthorizeAsync(AuthorizationCodeReceivedNotification context)
        {
            var email = IdentityHelper.GetUsername(context.AuthenticationTicket.Identity);
            var result = AuthorizeResult.Deny;

            foreach (var agent in agents)
            {
                var agentResult = await agent.AuthorizeAsync(context);

                Log.Information("Authorization agent {agent} returned {result} for user {user}", agent.GetType().Name, agentResult, email);

                // if AllowConditinal was previously set, it should not be overwritten by DenyConditional
                if (result != AuthorizeResult.AllowConditional || agentResult != AuthorizeResult.DenyConditional)
                {
                    result = agentResult;
                }

                if (result == AuthorizeResult.Allow || result == AuthorizeResult.Deny)
                {
                    // skip evaluation of next agents
                    break;
                }
            }

            Log.Information("Final result from authorization agents for user {user}: {result}", email, result);

            if (result == AuthorizeResult.Deny || result == AuthorizeResult.DenyConditional)
            {
                var authException = new SecurityTokenValidationException();
                authException.Data["userEmail"] = email;
                throw authException;
            }
        }
    }
}