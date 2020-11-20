using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace Webscraper
{
    public class Browser : IDisposable
    {
        private readonly ChromeDriver _driver = CreateDriver();

        private static ChromeDriver CreateDriver()
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--disable-dev-shm-usage");
            //options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            //options.AddUserProfilePreference("profile.managed_default_content_settings.javascript", 2);
            var driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 0, 0, 500);
            return driver;
        }

        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public IEnumerable<IWebElement> QuerySelectorAll(string cssSelector) =>
            _driver.FindElements(By.CssSelector(cssSelector));

        #region IDisposable
        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                _driver.Close();
                disposed = true;
            }
        }

        ~Browser()
        {
            Dispose();
        }
        #endregion
    }
}
