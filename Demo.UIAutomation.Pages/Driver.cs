using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace Demo.UIAutomation.Pages
{
    public class Driver
    {
        private static readonly ThreadLocal<IWebDriver> ThreadLocalInstance = new ThreadLocal<IWebDriver>(); 
        private static readonly object Padlock = new object();

        private Driver()
        {}

        public static IWebDriver Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (ThreadLocalInstance.Value == null)
                    {
                        //ThreadLocalInstance.Value = new PhantomJSDriver();
                        ThreadLocalInstance.Value = new ChromeDriver();
                        ThreadLocalInstance.Value.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                    }
                        
                    return ThreadLocalInstance.Value;
                }
            }
        }

        public static void Close()
        {
            ThreadLocalInstance.Value.Dispose();
            ThreadLocalInstance.Value = null;
        }
    }
}