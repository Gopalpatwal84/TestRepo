using Acom.Selenium.Tests.ObjectModel.Components;

using TestStack.Seleno.PageObjects;

namespace Acom.Selenium.Tests.ObjectModel
{
    public class Layout<T> : Page where T : Layout<T>
    {
        public Menu Menu
        {
            get
            {
                return this.GetComponent<Menu>();
            }
        }
    }
}