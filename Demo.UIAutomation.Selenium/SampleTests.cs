using System;
using System.Threading;
using System.Threading.Tasks;
using Bumblebee.Setup;
using Demo.UIAutomation.Pages;
using Demo.UIAutomation.Pages.Bumble;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;

namespace Demo.UIAutomation.Selenium
{
    [TestFixture]
    public class SampleTests
    {
        [Test]
        public void given_with_selenium()
        {
            //using (IWebDriver driver = new ChromeDriver())
            using (IWebDriver driver = new PhantomJSDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                //driver.Url = "file://C:/git/Demo.UIAutomation/Demo.UIAutomation.Selenium/TestPage.html";
                driver.Url = "https://www.nirvanahq.com/";

                //driver.FindElement(By.ClassName("login")).Click();
                //driver.FindElement(By.CssSelector("a.login")).Click();
                driver.FindElement(By.LinkText("Login")).Click();
                
                driver.FindElement(By.Id("emailaddress")).SendKeys("todd@meinershagen.net");
                driver.FindElement(By.Id("password")).SendKeys("jesusislord");
                driver.FindElement(By.Name("submit")).Click();

                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                //element = wait.Until((d) => d.FindElement(By.ClassName("next")));

                driver.FindElement(By.ClassName("next")).Click();

                const string expectedTaskName = "Test Task";
                driver.FindElement(By.Name("r2")).SendKeys(expectedTaskName + Keys.Return);

                var elements = driver.FindElement(By.ClassName("tasks")).FindElements(By.TagName("li"));
                var element = elements[elements.Count - 1];
                var taskName = element.Text;

                var target = driver.FindElement(By.ClassName("trash"));
                var builder = new Actions(driver);

                var dragAndDrop = builder.ClickAndHold(element)
                    .MoveToElement(target)
                    .Release(target)
                    .Build();

                dragAndDrop.Perform();

                Assert.That(taskName, Is.EqualTo(expectedTaskName));
            }
        }

        //[Test]
        //public void given_with_coypu()
        //{
        //    var sessionConfiguration = new SessionConfiguration
        //    {
        //        Driver = typeof(SeleniumWebDriver),
        //        //Browser = Browser.Chrome,
        //        Browser = Browser.PhantomJS,
        //        Timeout = TimeSpan.FromSeconds(30)
        //    };

        //    using (var browser = new BrowserSession(sessionConfiguration))
        //    {
        //        //driver.Url = "file://C:/git/Demo.UIAutomation/Demo.UIAutomation.Selenium/TestPage.html";
        //        browser.Visit("https://www.nirvanahq.com/");

        //        //browser.FindCss("a.login", new Options{Match = Match.First}).Click();
        //        browser.ClickLink("Login");
        //        browser.FillIn("emailaddress").With("todd@meinershagen.net");
        //        browser.FillIn("password").With("jesusislord");
        //        browser.ClickButton("submit");
        //        //browser.FindCss("a.next").Click();
        //        browser.ClickLink("Next");
        //        const string expectedTaskName = "Test Task";

        //        //browser.FindCss("input.rapidentry").FillInWith(expectedTaskName + Keys.Return);
        //        //browser.FillIn("r2").With(expectedTaskName + Keys.Return);
        //        browser.FillIn("Rapid Entry (type here)").With(expectedTaskName + Keys.Return);

        //        var driver = browser.Native as IWebDriver;
        //        var elements = driver.FindElement(By.ClassName("tasks")).FindElements(By.TagName("li"));
        //        var element = elements[elements.Count - 1];
        //        var taskName = element.Text;

        //        var target = driver.FindElement(By.ClassName("trash"));
        //        var builder = new Actions(driver);

        //        var dragAndDrop = builder.ClickAndHold(element)
        //            .MoveToElement(target)
        //            .Release(target)
        //            .Build();

        //        dragAndDrop.Perform();

        //        Assert.That(taskName, Is.EqualTo(expectedTaskName));
        //    }
        //}

        [Test]
        public void given_with_pages()
        {
            using (var app = new App())
            {
                const string listName = "next";
                const string expectedTaskName = "Test Task";

                var mainPage = app
                    .Login("todd@meinershagen.net", "jesusislord")
                    .AddTask(listName, expectedTaskName);

                Assert.That(mainPage.TaskListContainsOnlyOne(listName, expectedTaskName), Is.True);

                mainPage.DeleteTask(listName, expectedTaskName);
            }
        }

        [Test]
        public void given_multiple_threads_when_adding_task_should_add_task()
        {
            Action action = () =>
            {
                using (var app = new App())
                {
                    const string listName = "next";
                    var expectedTaskName = string.Format("Test Task for Thread {0}", Thread.CurrentThread.ManagedThreadId);

                    var mainPage = app.Login("todd@meinershagen.net", "jesusislord");
                    mainPage.AddTask(listName, expectedTaskName);

                    Assert.That(mainPage.TaskListContainsOnlyOne(listName, expectedTaskName), Is.True);

                    mainPage.DeleteTask(listName, expectedTaskName);
                }
            };

            Parallel.Invoke(action, action, action);
        }

        //private readonly ThreadLocal<Session> _threadLocalSession = new ThreadLocal<Session>();

        //protected Session Session
        //{
        //    get { return _threadLocalSession.Value; }
        //    set { _threadLocalSession.Value = value; }
        //}

        [Test]
        public void given_with_bumblee()
        {
            //Session = new Session(new LocalChromeEnvironment());

            try
            {
                //Session
                Session<LocalChromeEnvironment>
                    .Current
                    .NavigateTo<LoginPage>("https://www.nirvanahq.com/account/login")
                    .Username.EnterText("todd@meinershagen.net")
                    .Password.EnterText("jesusislord")
                    .SignIn.Click();

            }
            finally
            {
                Session<LocalChromeEnvironment>.Reset();
            }
        }
    }

    public class LocalChromeEnvironment : IDriverEnvironment
    {
        public IWebDriver CreateWebDriver()
        {
            var driver = new ChromeDriver();
            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            return driver;
        }
    }
}
