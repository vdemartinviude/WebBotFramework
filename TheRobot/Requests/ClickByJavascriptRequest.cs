using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ClickByJavascriptRequest : IRobotRequest
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
        IJavaScriptExecutor javaScriptExecutor;
        IWebElement element;
        var wait = new WebDriverWait(driver, Timeout!.Value);
        element = wait.Until(d => driver.FindElement(By));
        javaScriptExecutor = (IJavaScriptExecutor)driver;
        javaScriptExecutor.ExecuteScript("arguments[0].click();", element);
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}