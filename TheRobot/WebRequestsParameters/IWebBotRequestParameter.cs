using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using TheRobot.RequestsParameters;

namespace TheRobot.WebRequestsParameters;

public interface IWebBotRequestParameter : IRequestParameter
{
    public TimeSpan? Timeout { get; set; }
    public TimeSpan? DelayBefore { get; set; }
    public TimeSpan? DelayAfter { get; set; }
}