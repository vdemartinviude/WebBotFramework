using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class IsPageLoadedRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var docLoaded = driver.ExecuteJavaScript<bool>("return document.readyState === 'ready';");

        return new()
        {
            Status = docLoaded ? RobotResponseStatus.ActionRealizedOk : RobotResponseStatus.ElementNotFound
        };
    }
}