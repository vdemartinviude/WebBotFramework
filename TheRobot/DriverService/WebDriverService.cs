using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;
using Microsoft.Extensions.Logging;
using TheRobot.Responses;
using OpenQA.Selenium.Support.UI;
using OneOf;
using TheRobot.MediatedRequests;
using System.Runtime.CompilerServices;
using OpenQA.Selenium.Interactions;

namespace TheRobot.DriverService;

public class WebDriverService : IDisposable
{
    private readonly ILogger<WebDriverService> _logger;
    private readonly WebDriver _webDriver;

    public string DownloadFolder { get; private set; }
    public string CurrentUrl { get => _webDriver.Url; }
    public string CurrentPageTitle { get => _webDriver.Title; }

    public WebDriverService(ILogger<WebDriverService> logger)
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        DownloadFolder = Path.GetTempPath();

        ChromeOptions options = new();

        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.default_directory", DownloadFolder);
        options.AddArgument("--log-level=OFF");
        options.AddExcludedArgument("enable-logging");

        logger!.LogInformation("Starting the selenium driver");

        _webDriver = new ChromeDriver(options);

        _webDriver.Manage().Window.Maximize();
        _logger = logger;
    }

    public OneOf<ErrorOnWebAction, SuccessOnWebAction> NavigateTo(string url)
    {
        try
        {
            _webDriver.Navigate().GoToUrl(url);
            return new SuccessOnWebAction();
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction()
            {
                Error = ex.Message
            };
        }
    }

    public OneOf<ErrorOnWebAction, SuccessOnWebAction> QuitDriver()
    {
        try
        {
            _webDriver.Quit();
            return new SuccessOnWebAction();
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction() { Error = ex.Message };
        }
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> ChangeFrame(TimeSpan timeout, By by, CancellationToken token)
    {
        try
        {
            IWebElement element = await GetWebElement(timeout, by, token);
            _webDriver.SwitchTo().Frame(element);
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction
            {
                Error = ex.Message
            };
        }
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Click(TimeSpan timeout, By by, KindOfClik kind, CancellationToken token)
    {
        try
        {
            IWebElement element = await GetWebElement(timeout, by, token);
            switch (kind)
            {
                case KindOfClik.ClickByDriver:
                    element.Click();
                    break;

                case KindOfClik.ClickByJavaScriptWithoutFocus:
                case KindOfClik.ClickByJavaScriptWithFocus:
                    _webDriver.ExecuteScript("arguments[0].click();", element);
                    break;
            }
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction()
            {
                Error = ex.Message
            };
        }
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> ScrollToElement(TimeSpan timeout, By by, CancellationToken token)
    {
        try
        {
            IWebElement element = await GetWebElement(timeout, by, token);
            new Actions(_webDriver)
                .ScrollToElement(element)
                .Perform();
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction()
            {
                Error = ex.Message
            };
        }
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> MarkDownElement(TimeSpan timeout, By by, string markDownColor, CancellationToken token)
    {
        try
        {
            IWebElement element = await GetWebElement(timeout, by, token);
            _webDriver.ExecuteScript($"arguments[0].style.background='{markDownColor}';", element);
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction
            {
                Error = ex.Message
            };
        }
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> SetText(KindOfSetText kindOfSetText, string textToSet, TimeSpan timeOut, By? by, int? numberOfBackSpaces, CancellationToken cancellationToken)
    {
        try
        {
            IWebElement element = await GetWebElement(timeOut, by, cancellationToken);

            switch (kindOfSetText)
            {
                case KindOfSetText.SetByWebDriver:
                    element.SendKeys(textToSet);
                    break;

                case KindOfSetText.SetWithBackSpaceAndKeyPress:
                    await StringPress(element, new string('\b', numberOfBackSpaces!.Value), cancellationToken);
                    await StringPress(element, textToSet, cancellationToken);
                    break;

                case KindOfSetText.SetWithKeyPress:
                    await StringPress(element, textToSet, cancellationToken);
                    break;

                case KindOfSetText.SetWithJs:
                    _webDriver.ExecuteScript($"arguments[0].value='{textToSet}';", element);
                    break;
            }
            return new SuccessOnWebAction { WebElement = element };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction { Error = ex.Message };
        }
    }

    private async Task StringPress(IWebElement element, string text, CancellationToken token)
    {
        var actions = new Actions(_webDriver);
        var rnd = new Random();
        actions.Click(element);
        foreach (var c in text)
        {
            string s = new string(c, 1);
            actions.KeyDown(s);
            actions.Pause(TimeSpan.FromMilliseconds(rnd.Next(10, 30)));
            actions.KeyUp(s);
            actions.Pause(TimeSpan.FromMilliseconds(rnd.Next(30, 60)));
        }
        await Task.Run(() => actions.Perform(), token);
    }

    private async Task<IWebElement> GetWebElement(TimeSpan timeout, By by, CancellationToken token)
    {
        var wait = new WebDriverWait(_webDriver, timeout);
        var element = await Task.Run(() => { return wait.Until(drv => drv.FindElement(by)); }, token);
        return element;
    }

    public void Dispose()
    {
        _logger.LogCritical("Encerrando driver!");
        _webDriver.Quit();
    }
}