
using Microsoft.Owin;
using Owin;
using PowerBI.Jobs.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace PowerBI.Jobs.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (GlobalAuthFilterConfig.AuthEnabled())
            {
                ConfigureAuth(app);
            }
        }
    }
}