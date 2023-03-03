using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.MediatedRequests;

public class GenericMediatedParameters
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public TimeSpan TimeOut { get; set; }
}