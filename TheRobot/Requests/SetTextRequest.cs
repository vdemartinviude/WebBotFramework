using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SetTextRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? By { get; set; }
    public string? Text { get; set; }
    public TimeSpan? Timeout { get; set; }
    public bool? ClearBefore { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (By == null)
        {
            throw new ArgumentNullException(nameof(By));
        }
        if (Text == null)
        {
            throw new ArgumentNullException(nameof(Text));
        }
        if (driver == null)
        {
            throw new ApplicationException("WebDriver error");
        }
        IWebElement webElement;

        WebDriverWait wait = new WebDriverWait(driver, Timeout!.Value);
        webElement = wait.Until(e => e.FindElement(By));

        if (ClearBefore.HasValue && ClearBefore.Value)
        {
            webElement.Clear();
        }
        Task.Delay(100).Wait();
        var rnd = new Random();
        webElement?.Click();
        Task.Delay(100).Wait();
        foreach (var c in Text)
        {
            webElement?.SendKeys(c.ToString());
            Task.Delay(rnd.Next(100, 200)).Wait();
        }

        return new RobotResponse()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}