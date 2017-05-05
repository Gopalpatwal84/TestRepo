using System;
using System.Threading.Tasks;
using System.Web;
using Mediation.Interceptors;
using PowerBI.Messages.Models;

namespace PowerBI.Web.Infrastructure.Mediator
{
    public class TrackingCookieInterceptor : OutboundInterceptorBase
    {
        private readonly Func<HttpRequestBase> request;

        public TrackingCookieInterceptor(Func<HttpRequestBase> request)
        {
            this.request = request;
        }

        public override async Task OnCommandSending<TCommand>(TCommand command)
        {
            var trackingCommand = command as ITrackingCookie;
            if (trackingCommand != null && request != null)
            {
                trackingCommand.TrackingCookie = request()
                    ?.Cookies
                    ?.Get("_mkto_trk")
                    ?.Value;
            }

            await Task.FromResult(0);
        }
    }
}