using Microsoft.Extensions.Logging;
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

public class ClickRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public By? By { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (By == null)
        {
            throw new ArgumentNullException("By", "You must specify the element to click");
        }
        var wait = new WebDriverWait(driver, Timeout!.Value);

        var element = wait.Until(d => d.FindElement(By));
        new Actions(driver)
            .ScrollToElement(element)
            .Perform();
        element.Click();
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}