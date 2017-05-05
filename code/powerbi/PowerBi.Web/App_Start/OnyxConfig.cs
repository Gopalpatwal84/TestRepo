using System.IO;
using System.Web;
using Acom.Mvc.Svg;

namespace PowerBI.Web
{
    public static class OnyxConfig
    {
        public static void RegisterSvgs(SvgCollection svgs)
        {
            foreach (var svg in Directory.GetFiles(HttpContext.Current.Server.MapPath("/svg/")))
            {
                svgs.AddSvg(File.ReadAllText(svg));
            }
        }
    }
}