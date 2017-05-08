[assembly: Microsoft.Owin.OwinStartup(typeof(BusinessPlatform.Jobs.Web.Startup))]

namespace BusinessPlatform.Jobs.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (GlobalAuthFilterConfig.AuthEnabled())
            {
                this.ConfigureAuth(app);
            }
        }
    }
}