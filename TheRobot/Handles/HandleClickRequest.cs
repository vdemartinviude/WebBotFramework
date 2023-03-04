using MediatR;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleMediatedClickRequest : IRequestHandler<MediatedClickRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleMediatedClickRequest(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedClickRequest request, CancellationToken cancellationToken)
    {
        if (request.BaseParameters == null)
        {
            throw new ArgumentNullException("BaseParameters");
        }
        if (request.BaseParameters.By == null)
        {
            throw new ArgumentNullException("By");
        }
        Stopwatch stopwatch = Stopwatch.StartNew();

        var actionresult = await Task.Run(() => _handleactions(request.BaseParameters.TimeOut, request.BaseParameters.By, request.Kind, cancellationToken, request.BaseParameters?.ElementAlreadyFound));
        if (actionresult.IsT1)
        {
            stopwatch.Stop();
            actionresult.AsT1.ElapsedTime = stopwatch.Elapsed;
        }
        else
        {
            stopwatch.Stop();
        }
        return actionresult;
    }

    private OneOf<ErrorOnWebAction, SuccessOnWebAction> _handleactions(TimeSpan timeout, By by, KindOfClik kind, CancellationToken token, IWebElement? elementAlreadyFound)
    {
        try
        {
            if (elementAlreadyFound == null)
            {
                var wait = new WebDriverWait(_webDriverService.GetWebDriver(), timeout);
                elementAlreadyFound = wait.Until(drv => drv.FindElement(by), token);
            }

            switch (kind)
            {
                case KindOfClik.ClickByDriver:
                    elementAlreadyFound.Click();
                    break;

                case KindOfClik.ClickByJavaScriptWithFocus:
                case KindOfClik.ClickByJavaScriptWithoutFocus:
                    _webDriverService.GetWebDriver().ExecuteScript("arguments[0].click();", elementAlreadyFound);
                    break;
            }
            return new SuccessOnWebAction
            {
                WebElement = elementAlreadyFound,
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
}