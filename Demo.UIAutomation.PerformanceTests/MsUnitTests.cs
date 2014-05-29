using System;
using System.Threading;
using Demo.UIAutomation.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Demo.UIAutomation.PerformanceTests
{
    [TestClass]
    [DeploymentItem("phantomjs.exe")]
    public class MsUnitTests
    {
        [TestMethod]
        public void AddingTasks()
        {
            using (var app = new App())
            {
                const string listName = "next";
                var expectedTaskName = string.Format("Test Task for Thread {0}", Thread.CurrentThread.ManagedThreadId);

                var mainPage = app
                    .Login("todd@meinershagen.net", "jesusislord")
                    .AddTask(listName, expectedTaskName);

                Assert.That(mainPage.TaskListContainsOnlyOne(listName, expectedTaskName), Is.True);

                mainPage.DeleteTask(listName, expectedTaskName);
            }
        }
    }
}
