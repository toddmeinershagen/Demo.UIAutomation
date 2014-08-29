using System;
using OpenQA.Selenium;

namespace Demo.UIAutomation.Pages
{
    public class App : IDisposable
    {
        public App()
        {
            Driver.Instance.Navigate().GoToUrl("http://www.nirvanahq.com");
        }

        public MainPage Login(string username, string password)
        {
            Driver.Instance.FindElement(By.LinkText("Login")).Click();

            Driver.Instance.FindElement(By.Id("emailaddress")).SendKeys(username);
            Driver.Instance.FindElement(By.Id("password")).SendKeys(password);
            Driver.Instance.FindElement(By.Name("submit")).Click();

            return new MainPage();
        }

        public void Close()
        {
            Driver.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }

    public class AppContext
    {
        public string Host { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
