using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Demo.UIAutomation.Pages
{
    public class MainPage
    {
        public MainPage AddTask(string listName, string taskName)
        {
            Driver.Instance.FindElement(By.ClassName(listName)).Click();
            Driver.Instance.FindElement(By.Name("r2")).SendKeys(taskName + Keys.Return);

            return this;
        }

        public IEnumerable<string> GetTaskList(string listName)
        {
            Driver.Instance.FindElement(By.ClassName(listName)).Click();
            var elements = Driver.Instance.FindElement(By.ClassName("tasks")).FindElements(By.TagName("li"));
            return elements.Select(x => x.Text);
        }

        public bool TaskListContainsOnlyOne(string listName, string taskName)
        {
            var tasks = GetTaskList(listName);
            return tasks.Count(x => x == taskName) == 1;
        }

        public MainPage DeleteTask(string listName, string taskName)
        {
            var elements = GetTaskElements(listName);
            var element = elements.FirstOrDefault(x => x.Text == taskName);

            var target = Driver.Instance.FindElement(By.ClassName("trash"));
            var builder = new Actions(Driver.Instance);

            var dragAndDrop = builder.ClickAndHold(element)
                .MoveToElement(target)
                .Release(target)
                .Build();

            dragAndDrop.Perform();

            return this;
        }

        private IEnumerable<IWebElement> GetTaskElements(string listName)
        {
            Driver.Instance.FindElement(By.ClassName(listName)).Click();
            return Driver.Instance.FindElement(By.ClassName("tasks")).FindElements(By.TagName("li"));
        }
    }
}