using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.RequestsParameters;
using TheRobot.Response;

namespace TheRobot.WebRequestsParameters;

public class NavigateRequestParameters : IWebBotRequestParameter
{
    public string? Url { get; set; }
    public TimeSpan? Timeout { get; set; }
    public TimeSpan? DelayBefore { get; set; }
    public TimeSpan? DelayAfter { get; set; }
}