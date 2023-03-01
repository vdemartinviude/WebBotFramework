using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.Requests;

public class SelectRangeByValueRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public By? ByClick { get; set; }
    public By? BySelectValues { get; set; }
    public bool GreaterThan { get; set; }
    public bool LessThan { get; set; }
    public TimeSpan? DelayBetweenClicks { get; set; }
    public double Value { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        IWebElement firstClickElement;
        firstClickElement = new WebDriverWait(driver, Timeout!.Value).Until(x => x.FindElement(ByClick));
        DelayBetweenClicks ??= TimeSpan.FromSeconds(1);
        var actions = new Actions(driver);
        actions.ScrollToElement(firstClickElement);
        actions.ScrollByAmount(0, 100);
        actions.Perform();
        Thread.Sleep((TimeSpan)DelayBetweenClicks);
        firstClickElement.Click();
        Thread.Sleep((TimeSpan)DelayBetweenClicks);
        var elements = driver.FindElements(BySelectValues)
                       .Select(element => new
                       {
                           element = element,
                           valor = Convert.ToDouble(
                           System.Text.RegularExpressions.Regex.Match(element.Text, @"[\d\.,]+").Value, new CultureInfo("pt-BR"))
                       });
        IWebElement? element = null;
        if (GreaterThan)
        {
            element = elements.OrderBy(a => a.valor).Where(a => a.valor >= Value).Select(a => a.element).First();
        }
        if (LessThan)
        {
            element = elements.OrderByDescending(a => a.valor).Where(a => a.valor <= Value).Select(a => a.element).First();
        }
        element!.Click();
        return new() { Status = RobotResponseStatus.ActionRealizedOk };
    }
}