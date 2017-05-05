using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;

using TestStack.Seleno.PageObjects;

using By = TestStack.Seleno.PageObjects.Locators.By;

namespace Acom.Selenium.Tests.ObjectModel.Components
{
    public class Menu : UiComponent
    {
        private string BaseSelector
        {
            get
            {
                return "ul.navbar-nav";
            }
        }

        public IEnumerable<IWebElement> Items
        {
            get
            {
                return this.Find.Elements(By.jQuery(this.BaseSelector + "li a"));
            }
        }
    }
}