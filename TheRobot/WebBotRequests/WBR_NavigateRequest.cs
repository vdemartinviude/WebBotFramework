using OneOf;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.RequestsParameters;
using TheRobot.WebBotInterfaces;
using TheRobot.WebRequestsParameters;

namespace TheRobot.WebBotRequests;

public class WBR_NavigateRequest : IWebBotRequest
{
    public OneOf<ErrorOnWebAction, SuccessOnWebAction, IWebElement, IEnumerable<IWebElement>> ExecRequest(IWebDriver driver, IWebBotRequestParameter parameters)
    {
        if (parameters is not NavigateRequestParameters navigationParameters)
        {
            throw new ArgumentException("Invalid parameter type", nameof(parameters));
        }

        if (driver == null)
        {
            throw new ArgumentNullException("Driver");
        }
        driver.Navigate().GoToUrl(navigationParameters.Url);
        return new SuccessOnWebAction();
    }
}