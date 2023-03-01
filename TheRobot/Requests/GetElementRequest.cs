using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Logging;

namespace TheRobot.Requests;

public class GetElementRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public By? By { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        IWebElement? webElement = null;
        if (By == null)
        {
            throw new ArgumentNullException(nameof(By));
        }

        WebDriverWait wait = new WebDriverWait(driver, Timeout!.Value);
        webElement = wait.Until(e => e.FindElement(By));

        return new()
        {
            WebElement = webElement,
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}