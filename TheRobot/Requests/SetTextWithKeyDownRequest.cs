using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetTextWithKeyDownRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public string? Text { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var element = driver.FindElement(By);
        var firstactions = new Actions(driver);
        firstactions.ScrollToElement(element);
        firstactions.Click(element);
        firstactions.Perform();
        foreach (var c in Text!)
        {
            string s = String.Empty;
            s += c;
            var actions = new OpenQA.Selenium.Interactions.Actions(driver);

            actions.KeyDown(s);
            actions.Pause(TimeSpan.FromMilliseconds(250));
            actions.KeyUp(s);
            actions.Pause(TimeSpan.FromMilliseconds(250));
            actions.Perform();
        }
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}