using Microsoft.Owin;
using Owin;
using PowerBI.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace PowerBI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (FilterConfig.AuthEnabled())
            {
                ConfigureAuth(app);
            }
        }
    }
}
