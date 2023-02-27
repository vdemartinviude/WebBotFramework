using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class WaitAndMoveElementSelectable : IRobotRequest
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
        var wait = new WebDriverWait(driver, Timeout!.Value)
             .Until(ExpectedConditions.ElementExists(By));

        if (wait == null) throw new NoSuchElementException();

        IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
        executor.ExecuteScript("arguments[0].scrollIntoView();", wait);
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk,
            WebElement = wait
        };
    }
}