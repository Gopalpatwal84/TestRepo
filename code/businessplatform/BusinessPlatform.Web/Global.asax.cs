namespace BusinessPlatform.Web
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Helpers;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Acom.Forms;
    using Acom.IO.Entities;
    using Acom.Mvc;
    using Acom.Mvc.Bundles;
    using Acom.Mvc.Helpers;
    using Acom.Mvc.Minify;
    using ChameleonForms;
    using Controllers;
    using Features.UserInfoCookie;
    using Serilog;

    public class MvcApplication : System.Web.HttpApplication
    {
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            // NOTE: Same output cache vary for all profiles

            // Tack on an additional vary if you have the cdn disabled for your session
            var cdnCookie = context.IsCdnDisabled();

            // This custom Vary param is used to force output caching to be refreshed when swapping from
            //  staging to prodution environments
            return "server=" + context.Request.Params["SERVER_NAME"] +
                    "-version=" + Acom.Configuration.KnownDeploymentVariables.DeploymentVersion +
                    "-cdn=" + cdnCookie;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            var container = ContainerConfig.RegisterIoC();
            OnyxConfig.RegisterSvgs(OnyxTable.Svgs);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.ConfigureBundles(container, BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            LoggerConfig.ConfigureLogging();

            GlobalFilters.Filters.Add(new UserInfoCookieFilter());

            var razor = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            razor.ViewLocationFormats = razor.ViewLocationFormats.Concat(new[] { "~/Views/_global/{0}.cshtml", "~/Views/_global/{1}/{0}.cshtml", "~/Views/_global/{0}/index.cshtml" }).ToArray();
            razor.PartialViewLocationFormats = razor.PartialViewLocationFormats.Concat(new[] { "~/Views/_shared/{0}.cshtml", "~/Views/_global/{0}.cshtml", "~/Views/_global/{0}/index.cshtml" }).ToArray();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            MvcHandler.DisableMvcResponseHeader = true;

            Log.Warning("App Started");

            FormTemplate.Default = OnyxDelegateFormTemplate.Instance;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.QueryString.Get("cdn") == "disable")
            {
                HttpContext.Current.SetCdnDisabledCookie();
                Log.Warning("CDN FALLING BACK");
            }
        }

        protected void Application_PostAuthorizeRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            if (!context.IsDebuggingEnabled && context.Response.BufferOutput && context.Response.Buffer)
            {
                context.Response.Filter = new HtmlMinificationFilter(context);
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!this.CustomErrorsEnabled())
            {
                // Let the YSOD shine through
                return;
            }

            try
            {
                var exception = Server.GetLastError();

                // Output current exception to application logs
                Log.Error(exception, "Application Error");
                Server.ClearError();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "ErrorController");
                routeData.Values.Add("culture", KnownCultures.Current);
                routeData.Values.Add("action", "Error");

                // This doesn't work for HttpExceptions - maybe we can replace this whole section with an expanded customErrors sections

                // Call the controller with the route
                IController controller = new ErrorController();
                controller.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));
            }
            catch (Exception ex)
            {
                // Output current exception to application logs
                Log.Error(ex, "Application Error Handler Exception");

                // Trap exception and avoid recursive exception handling.
                // Return a simple error response for this scenario.
                Response.Clear();
                Response.Write("Something went wrong.  Please check your url and try again.");
                Response.StatusCode = 500;
                Response.End();
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        private bool CustomErrorsEnabled()
        {
            var customErrorsSection = ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection;

            if (customErrorsSection == null)
            {
                return false;
            }

            return customErrorsSection.Mode == CustomErrorsMode.On ||
                (customErrorsSection.Mode == CustomErrorsMode.RemoteOnly && !Request.IsLocal);
        }
    }
}
