using System;
using System.Runtime.CompilerServices;

using Acom.Selenium.Tests.Utils;

using Humanizer;

using TestStack.Seleno.PageObjects;

namespace Acom.Selenium.Tests.Fixtures
{
    public static class Extensions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T CheckWindow<T>(this T component, string checkpointName = null) where T : UiComponent
        {
            if (string.IsNullOrEmpty(checkpointName)) checkpointName = Host.Instance.Application.Browser.Title;

            if (TestConfiguration.Instance.EnableAppliTools)
                Host.Eyes.CheckWindow(checkpointName.Humanize());

            return component;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CheckWindowAndClose<T>(this T component, [CallerMemberName] string checkpointName = null) where T : UiComponent
        {
            if (string.IsNullOrEmpty(checkpointName)) checkpointName = Host.Instance.Application.Browser.Title;

            if (TestConfiguration.Instance.EnableAppliTools)
                Host.Eyes.CheckWindow(String.Format("{0} - {1}", checkpointName.Humanize(), Host.Instance.Application.Browser.Url));

            Host.Eyes.Close();
        }
    }
}