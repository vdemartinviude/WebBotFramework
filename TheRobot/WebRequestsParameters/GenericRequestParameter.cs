﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.WebRequestsParameters;

public class GenericRequestParameter : IWebBotRequestParameter
{
    public TimeSpan? Timeout { get; set; }
    public TimeSpan? DelayBefore { get; set; }
    public TimeSpan? DelayAfter { get; set; }
}