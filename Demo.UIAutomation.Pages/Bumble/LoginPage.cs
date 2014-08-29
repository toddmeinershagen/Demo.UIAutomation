using System;
using Bumblebee.Extensions;
using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Demo.UIAutomation.Pages.Bumble
{
    public class LoginPage : WebBlock
    {
        public LoginPage(Session session)
            : base(session)
        {}

        public ITextField<LoginPage> Username
        {
            get { return new TextField<LoginPage>(this, By.Id("username")); }
        }

        public ITextField<LoginPage> Password
        {
            get { return new TextField<LoginPage>(this, By.Id("password")); }
        }

        public IClickable<MainPage> SignIn
        {
            get { return new Clickable<MainPage>(new MainPage(Session), By.ClassName("submit")); }
        }
    }

    public class MainPage : WebBlock
    {
        public MainPage(Session session)
            : base(session)
        { }
    }

    public class WebBlock : Block
    {
        protected WebDriverWait Wait { get; private set; }

        public WebBlock(Session session)
            : base(session)
        {
            // Wait for the DOM to start changing so we can START waiting for the new element
            this.Pause(200);
            Wait = new WebDriverWait(Session.Driver, new TimeSpan(3000));
            Tag = Wait.Until(driver => driver.GetElement(By.TagName("body")));
        }
    }
}
