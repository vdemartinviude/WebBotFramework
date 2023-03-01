using Microsoft.Extensions.Logging;
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

public class WaitAndMoveToElementClickableRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var wait = new WebDriverWait(driver, Timeout!.Value)
            .Until(ExpectedConditions.ElementExists(By));
        IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
        executor.ExecuteScript("arguments[0].scrollIntoView();", wait);
        var wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(15))
            .Until(ExpectedConditions.ElementToBeClickable(By));

        return new()
        {
            WebElement = wait,
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}