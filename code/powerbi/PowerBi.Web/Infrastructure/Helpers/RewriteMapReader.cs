using System.Collections.Generic;
using System.Web.Hosting;
using System.Xml;
using Acom.IO.FileReaders;

namespace PowerBI.Web.Infrastructure.Helpers
{
    public class RewriteMapReader
    {
        private const string rewriteMapsFileName = "rewriteMaps.config";

        public Dictionary<string, string> Maps;

        public RewriteMapReader(string rewriteMapName)
        {
            Maps = new Dictionary<string, string>();

            var reader = new FileReader(HostingEnvironment.MapPath("~/config"));
            if (reader.Exists(rewriteMapsFileName))
            {
                var rewriteMaps = reader.ReadAllText(rewriteMapsFileName);
                var rewriteMapsXml = new XmlDocument();
                rewriteMapsXml.LoadXml(rewriteMaps);

                var rewriteMapNode = rewriteMapsXml.DocumentElement.SelectSingleNode(string.Format("rewriteMap[@name='{0}']", rewriteMapName));
                foreach (XmlNode config in rewriteMapNode.ChildNodes)
                {
                    Maps.Add(config.Attributes["key"].Value, config.Attributes["value"].Value);
                }
            }
        }
    }
}