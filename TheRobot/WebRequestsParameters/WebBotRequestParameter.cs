using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using TheRobot.RequestsParameters;

namespace TheRobot.WebRequestsParameters;

public abstract class WebBotRequestParameter
{
    public abstract TimeSpan? Timeout { get; set; }
    public abstract TimeSpan? DelayBefore { get; set; }
    public abstract TimeSpan? DelayAfter { get; set; }
}