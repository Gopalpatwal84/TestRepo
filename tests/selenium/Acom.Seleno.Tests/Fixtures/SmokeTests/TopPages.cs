using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestConfiguration = Acom.Selenium.Tests.TestConfiguration;

namespace Acom.Selenium.Tests.Fixtures.SmokeTests
{
    [TestClass]
    public class TopPages
    {
        [TestMethod]
        public void HomePage()
        {
            using (var session = new EyesSession(typeof(TopPages)))
            {
                string[] topPages =
                    {
                        @"/en-us/",
                        @"/ja-jp/",
                        @"/es-es/",
                        @"/fr-fr/",
                    };

                foreach (var item in topPages)
                {
                    this.CaptureUrl(item, session);
                }
            }
        }

        [TestMethod]
        public void PricingPages()
        {
            using (var session = new EyesSession(typeof(TopPages)))
            {
                string[] topPages =
                    {
                        @"/en-us/pricing/",
                    };

                foreach (var item in topPages)
                {
                    this.CaptureUrl(item, session);
                }
            }
        }

        [TestMethod]
        public void Blog()
        {
            using (var session = new EyesSession(typeof(TopPages)))
            {
                string[] topPages =
                    {
                        @"/en-us/blog/",
                    };

                foreach (var item in topPages)
                {
                    this.CaptureUrl(item, session);
                }
            }
        }

        [TestMethod]
        public void Existing()
        {
            using (var session = new EyesSession(typeof(TopPages)))
            {
                string[] topPages =
                    {
                        "/android-3rd-party/",
                        "/android-license-terms/",
                        "/androidterms/",
                        "/api-terms/",
                        "/custom-visuals/",
                        "/departments/",
                        "/designer/",
                        "/desktop-thank-you/",
                        "/desktop/",
                        "/developers/",
                        "/documentation/",
                        "/downloads/",
                        "/excel-dashboard-publisher-thank-you/",
                        "/excel-dashboard-publisher/",
                        "/features/",
                        "/gateway/",
                        "/industries/",
                        "/ios-3rd-party/",
                        "/ios-license-terms/",
                        "/knowledgebase/",
                        "/landing/excel/",
                        "/landing/pyramid/",
                        "/landing/sales/",
                        "/landing/signin/",
                        "/mobile/",
                        "/newsletter/",
                        "/partner-showcase/",
                        "/partners/",
                        "/publishtoweb/",
                        "/roles/",
                        "/solutions/",
                        "/support/",
                        "/terms-of-service/",
                        "/tour/",
                        "/trial-terms/",
                        "/visuals-gallery-terms/",
                        "/what-is-power-bi/",
                        "/win-phone-3rd-party/",
                        "/win-phone-license-terms/",
                        "/windows-3rd-party/",
                        "/windows-license-terms/",
                        "/winterms/",
                    };

                foreach (var item in topPages)
                {
                    this.CaptureUrl(string.Concat("/en-us", item), session);
                }
            }
        }

        private void CaptureUrl(string url, EyesSession session)
        {
            session.GoToUrl(TestConfiguration.Instance.EnvironmentUrl + url);
            session.CheckWindow();
        }
    }
}