using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class WaitElementExistsOrVanishRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public By By { get; set; }
    public bool? WaitVanish { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        IWebElement? element = null;
        bool found = false;
        bool condition = WaitVanish ?? false;
        if (!CancellationToken.HasValue)
        {
            throw new ArgumentNullException("CancelationToken");
        }
        do
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(500));
                element = wait.Until(d => d.FindElement(By));
                found = true;
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                found = false;
            }
        } while (!CancellationToken!.Value.IsCancellationRequested && (found == condition));
        if (!found)
        {
            return new()
            {
                Status = RobotResponseStatus.ElementNotFound
            };
        }
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk,
            WebElement = element
        };
    }
}