using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ChangeWindowByClickRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        try
        {
            string originalWindow = driver.CurrentWindowHandle;
            var wait = new WebDriverWait(driver, Timeout!.Value);
            var clickElement = wait.Until(d => d.FindElement(By));

            clickElement.Click();
            Thread.Sleep(3000);

            var wait2 = new WebDriverWait(driver, Timeout!.Value);
            var elements = wait2.Until(d => d.WindowHandles.Count == 2);
            foreach (string window in driver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    driver.SwitchTo().Window(window); break;
                }
            }
        }
        catch (Exception ex) when (ex is WebDriverTimeoutException ||
                                   ex is NoSuchElementException ||
                                   ex is NoSuchWindowException)
        {
            return new()
            {
                Status = RobotResponseStatus.ElementNotFound
            };
        }
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}