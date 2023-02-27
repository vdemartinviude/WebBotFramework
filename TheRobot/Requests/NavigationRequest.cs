using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class NavigationRequest : IRobotRequest
{
    public string? Url { get; set; }
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (String.IsNullOrEmpty(Url))
        {
            ArgumentNullException argumentNullException = new("Url", "The parameter must not be null");
            throw argumentNullException;
        }

        driver.Navigate().GoToUrl(Url);

        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}