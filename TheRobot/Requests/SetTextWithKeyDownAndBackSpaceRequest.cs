using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetTextWithKeyDownAndBackSpaceRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public string? Text { get; set; }
    public int? BackSpaceNumber { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        var element = driver.FindElement(By);
        var firstactions = new Actions(driver);
        BackSpaceNumber ??= 5;
        firstactions.ScrollToElement(element);
        firstactions.ScrollByAmount(0, 10);
        firstactions.Click(element);
        for (int i = 0; i < BackSpaceNumber; i++)
        {
            firstactions.KeyDown(Keys.Backspace);
            firstactions.Pause(TimeSpan.FromMilliseconds(150));
            firstactions.KeyUp(Keys.Backspace);
            firstactions.Pause(TimeSpan.FromMilliseconds(150));
        }

        firstactions.Perform();
        foreach (var c in Text!)
        {
            string s = String.Empty;
            s += c;
            var actions = new Actions(driver);
            actions.KeyDown(s);
            actions.Pause(TimeSpan.FromMilliseconds(150));
            actions.KeyUp(s);
            actions.Pause(TimeSpan.FromMilliseconds(150));
            actions.Perform();
        }
        return new() { Status = RobotResponseStatus.ActionRealizedOk };
    }
}