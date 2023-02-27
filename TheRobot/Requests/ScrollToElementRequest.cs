using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class ScrollToElementRequest : IRobotRequest
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
        IWebElement webElement = new WebDriverWait(driver, Timeout!.Value).Until(x => x.FindElement(By));

        var actions = new Actions(driver);
        actions.ScrollToElement(webElement);
        actions.ScrollByAmount(0, 10);
        actions.Perform();
        return new() { Status = RobotResponseStatus.ActionRealizedOk };
    }
}