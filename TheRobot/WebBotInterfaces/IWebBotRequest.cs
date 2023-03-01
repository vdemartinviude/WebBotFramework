using OneOf;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.WebRequestsParameters;

namespace TheRobot.WebBotInterfaces;

public interface IWebBotRequest
{
    public abstract OneOf<ErrorOnWebAction, SuccessOnWebAction, IWebElement, IEnumerable<IWebElement>> ExecRequest(IWebDriver driver, IWebBotRequestParameter parameters);
}