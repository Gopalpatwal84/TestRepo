using BusinessPlatform.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace BusinessPlatform.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (FilterConfig.AuthEnabled())
            {
                this.ConfigureAuth(app);
            }
        }
    }
}
