using System;
using System.Runtime.CompilerServices;

using Acom.Selenium.Tests.Utils;

namespace Acom.Selenium.Tests.Fixtures
{
    public class EyesSession : IDisposable
    {
        public EyesSession(Type testClass, [CallerMemberName] string checkpointName = null)
        {
            Host.SetupAppliTools(testClass, checkpointName);
        }

        public void GoToUrl(string url)
        {
            Host.Instance.Application.Browser.Navigate().GoToUrl(url);
        }

        public void CheckWindow()
        {
            if(TestConfiguration.Instance.EnableAppliTools)
                Host.Eyes.CheckWindow(Host.Instance.Application.Browser.Title + " - " + Host.Instance.Application.Browser.Url);
        }

        public void Dispose()
        {
            Host.Eyes.Close();
        }
    }
}