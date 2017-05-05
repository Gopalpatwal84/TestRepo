using System;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

using Acom.Selenium.Tests.Utils;

using Applitools;

using Humanizer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Extensions;

namespace Acom.Selenium.Tests
{
    [TestClass]
    public class Host
    {
        public static readonly SelenoHost Instance = new SelenoHost();
        private static string BranchName = new Uri(TestConfiguration.Instance.EnvironmentUrl).Host.Split('.').First();
        public static Size WindowSize = new Size(TestConfiguration.Instance.ScreenX, TestConfiguration.Instance.ScreenY);

        public static readonly Eyes Eyes = new Eyes
        {
            ApiKey = TestConfiguration.Instance.AppliToolsKey,
            MatchLevel = MatchLevel.Layout2,
            Batch = new BatchInfo(TestConfiguration.Instance.DefaultBatchName),
            ForceFullPageScreenshot = true,
            IsDisabled = !TestConfiguration.Instance.EnableAppliTools,
            SaveNewTests = true,
            StitchMode = StitchModes.CSS,
            HideScrollbars = true
        };

        static Host()
        {
            Instance.Run(configure => configure
                .WithWebServer(new InternetWebServer(TestConfiguration.Instance.EnvironmentUrl))
                .WithRemoteWebDriver(() =>
                    {
                        if (string.Equals("Local", TestConfiguration.Instance.Browser, StringComparison.OrdinalIgnoreCase))
                        {
                            return new ChromeDriver().SetWindowSize(WindowSize.Width, WindowSize.Height);
                        }
                        else if(string.Equals("PhantomJS", TestConfiguration.Instance.Browser, StringComparison.OrdinalIgnoreCase))
                        {
                            return BrowserFactory.PhantomJS().SetWindowSize(WindowSize.Width, WindowSize.Height);
                        }
                        return LoadRemoteWebDriver().SetWindowSize(WindowSize.Width, WindowSize.Height);
                    }));
        }

        static RemoteWebDriver LoadRemoteWebDriver()
        {
            var capabilities = DesiredCapabilities.Chrome();

            capabilities.SetCapability("browser", TestConfiguration.Instance.Browser);
            capabilities.SetCapability("browserName", TestConfiguration.Instance.Browser.ToLower());
            capabilities.SetCapability("os", TestConfiguration.Instance.Os);


            capabilities.SetCapability("browserstack.user", TestConfiguration.Instance.BrowserStackUser);
            capabilities.SetCapability("browserstack.key", TestConfiguration.Instance.BrowserStackKey);

            // Enable visual logs
            capabilities.SetCapability("browserstack.debug", "true");

            capabilities.SetCapability("project", TestConfiguration.Instance.Project);
            capabilities.SetCapability("build", DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmm"));

            // Session name
            capabilities.SetCapability("name", "Validation Tests");
            capabilities.SetCapability("browserTimeout", "120");

            //IE
            capabilities.SetCapability("ie.ensureCleanSession", true);

            return new ScreenshotRemoteWebDriver(new Uri(TestConfiguration.Instance.SeleniumServerHubUrl), capabilities);
        }

        public static void SetupAppliTools(Type testClass, [CallerMemberName] string checkpointName = null)
        {
            var fixtureName = testClass.Name.Humanize().Humanize();
            Host.Eyes.ParentBranchName = null;
            Host.Eyes.BranchName = BranchName;
            if (Host.Eyes.BranchName != TestConfiguration.Instance.RootBranchName)
                Host.Eyes.ParentBranchName = TestConfiguration.Instance.RootBranchName;
            var eyesDriver = Host.Eyes.Open(
                Host.Instance.Application.Browser,
                TestConfiguration.Instance.Project,
                fixtureName + " " + checkpointName.Humanize()
            ) as EyesWebDriver;
        }

        public static void CloseEyes()
        {
            try
            {
                Eyes.Close(false);
            }
            catch { }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Host.Eyes.AbortIfNotClosed();
        }
    }
}