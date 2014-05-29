using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace Demo.UIAutomation.Pages
{
    public class Driver
    {
        private static readonly ThreadLocal<IWebDriver> _instance = new ThreadLocal<IWebDriver>(); 
        private static readonly object Padlock = new object();

        private Driver()
        {}

        public static IWebDriver Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance.Value == null)
                    {
                        _instance.Value = new PhantomJSDriver();
                        _instance.Value.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                    }
                        
                    return _instance.Value;
                }
            }
        }

        public static void Close()
        {
            _instance.Value.Dispose();
            _instance.Value = null;
        }
    }
}