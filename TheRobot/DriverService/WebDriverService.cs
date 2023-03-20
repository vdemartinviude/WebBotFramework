using Microsoft.Extensions.Logging;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TheRobot.MediatedRequests;
using TheRobot.Responses;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> ChangeFrame(TimeSpan timeout, GenericWebElement by, CancellationToken token)
    {
        try
        {
            var element = await GetWebElement(timeout, by, token);
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Click(TimeSpan timeout, GenericWebElement by, KindOfClik kind, CancellationToken token)
    {
        IWebElement element;
        try
        {
            element = await GetWebElement(timeout, by, token);
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> ScrollToElement(TimeSpan timeout, GenericWebElement by, CancellationToken token)
    {
        IWebElement element;
        try
        {
            element = await GetWebElement(timeout, by, token);

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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> MarkDownElement(TimeSpan timeout, GenericWebElement by, string markDownColor, CancellationToken token)
    {
        try
        {
            var element = await GetWebElement(timeout, by, token);
            await Task.Run(() => _webDriver.ExecuteScript($"arguments[0].style.background='{markDownColor}';", element), token);
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (StaleElementReferenceException ex)
        {
            return new ErrorOnWebAction
            {
                Error = ex.Message
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> SetText(KindOfSetText kindOfSetText, string textToSet, TimeSpan timeOut, GenericWebElement by, int? numberOfBackSpaces, CancellationToken cancellationToken)
    {
        IWebElement element;
        try
        {
            element = await GetWebElement(timeOut, by, cancellationToken);
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> ElementExists(TimeSpan timeOut, GenericWebElement by, CancellationToken cancellationToken)
    {
        IWebElement element;
        try
        {
            element = await GetWebElement(timeOut, by, cancellationToken);
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> GetShadow(TimeSpan timeOut, GenericWebElement byOrElement, CancellationToken cancellationToken)
    {
        try
        {
            var hostElement = await GetWebElement(timeOut, byOrElement, cancellationToken);
            return new SuccessOnWebAction
            {
                SearchContext = hostElement.GetShadowRoot()
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> GetElements(TimeSpan timeout, GenericWebElement byOrElement, CancellationToken cancellation)
    {
        try
        {
            var elements = await GetWebElements(timeout, byOrElement, cancellation);
            return new SuccessOnWebAction
            {
                WebElements = elements
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> SelectByDriveByValue(TimeSpan timeOut, GenericWebElement byOrElement, string value, CancellationToken cancellationToken)
    {
        try
        {
            var element = await GetWebElement(timeOut, byOrElement, cancellationToken);
            var select = new SelectElement(element);
            select.SelectByValue(value);
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

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> SelectByDriveByText(TimeSpan timeOut, GenericWebElement byOrElement, string text, CancellationToken cancellationToken)
    {
        try
        {
            var element = await GetWebElement(timeOut, byOrElement, cancellationToken);
            var select = new SelectElement(element);
            select.SelectByText(text);
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

    private async Task StringPress(IWebElement element, string text, CancellationToken token)
    {
        var actions = new Actions(_webDriver);
        var rnd = new Random();
        actions.Click(element);
        foreach (var c in text)
        {
            string s = new(c, 1);
            actions.KeyDown(s);
            actions.Pause(TimeSpan.FromMilliseconds(rnd.Next(10, 30)));
            actions.KeyUp(s);
            actions.Pause(TimeSpan.FromMilliseconds(rnd.Next(30, 60)));
        }
        await Task.Run(() => actions.Perform(), token);
    }

    private async Task<IWebElement> GetWebElement(TimeSpan timeout, GenericWebElement GenElement, CancellationToken token)
    {
        var wait = new WebDriverWait(_webDriver, timeout);
        return await Task.Run(() =>
            GenElement.Match<IWebElement>(by => wait.Until(drv => drv.FindElement(by)),
                                  element => element,
                                  recursive => recursive.Element.FindElement(recursive.By),
                                  search => search.SearchContext.FindElement(By.CssSelector(search.CssSelector)),
                                  elementList => elementList.First()
                                  ), token);
    }

    private async Task<IEnumerable<IWebElement>> GetWebElements(TimeSpan timeout, GenericWebElement GenElement, CancellationToken token)
    {
        var wait = new WebDriverWait(_webDriver, timeout);
        return await Task.Run(() =>
            GenElement.Match<IEnumerable<IWebElement>>(by => wait.Until(drv => drv.FindElements(by)),
                                                       element => null,
                                                       recursive => recursive.Element.FindElements(recursive.By),
                                                       search => search.SearchContext.FindElements(By.CssSelector(search.CssSelector)),
                                                       elementList => elementList), token);
    }

    public void Dispose()
    {
        _logger.LogCritical("Encerrando driver!");
        _webDriver.Quit();
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> UploadFileBySelect(TimeSpan timeOut, GenericWebElement byOrElement, string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var element = await GetWebElement(timeOut, byOrElement, cancellationToken);
            element.SendKeys(filePath);
            return new SuccessOnWebAction
            {
                WebElement = element
            };
        }
        catch (Exception ex)
        {
            return new ErrorOnWebAction
            {
                Error = ex.Message,
            };
        }
    }
}