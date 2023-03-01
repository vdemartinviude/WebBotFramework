﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;

namespace TheRobot.RequestsParameters;

public interface IRequestParameter
{
    public TimeSpan? DelayBefore { get; set; }
    public TimeSpan? DelayAfter { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? Logger { get; set; }
}