﻿using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.RequestsParameters;

namespace TheRobot.WebRequestsParameters;

public class GenericWebRequestParameters : IWebBotRequestParameter
{
    public TimeSpan? Timeout { get; set; }
    public IWebDriver? Driver { get; set; }
    public TimeSpan? DelayBefore { get; set; }
    public TimeSpan? DelayAfter { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? Logger { get; set; }
}