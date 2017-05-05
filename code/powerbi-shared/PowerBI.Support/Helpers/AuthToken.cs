using System;
using System.Threading;
using System.Threading.Tasks;
using Acom.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Serilog;

namespace PowerBI.Support.Helpers
{
    public class AuthToken
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly AuthConfiguration _configuration;

        private string _token;
        private DateTimeOffset _expiry;

        public AuthToken(AuthConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetAuthToken()
        {
            if (_expiry < Clock.UtcNow() || string.IsNullOrEmpty(_token))
            {
                await this.RequestAuthToken();
            }

            return _token;
        }

        private async Task RequestAuthToken()
        {
            SemaphoreSlim.Wait();
            try
            {
                var authContext = new AuthenticationContext(string.Format(_configuration.AadInstance, _configuration.Tenant));
                var credential = new ClientCredential(_configuration.ClientId, _configuration.AppKey);

                var result = await authContext.AcquireTokenAsync(_configuration.ResourceId, credential);
                _token = result.AccessToken;
                _expiry = result.ExpiresOn;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get auth token");
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}
