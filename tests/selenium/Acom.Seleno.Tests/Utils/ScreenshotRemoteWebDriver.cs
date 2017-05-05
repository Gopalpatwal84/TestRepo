using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Acom.Selenium.Tests.Utils
{
    public class ScreenshotRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
    {
        public ScreenshotRemoteWebDriver(Uri uri, DesiredCapabilities dc)
            : base(uri, dc)
        {
        }

        public Screenshot GetScreenshot()
        {
            Response screenshotResponse = this.Execute(DriverCommand.Screenshot, null);
            string base64 = screenshotResponse.Value.ToString();
            return new Screenshot(base64);
        }
    }
}