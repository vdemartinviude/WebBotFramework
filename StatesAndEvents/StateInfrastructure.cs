using JsonDocumentsManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesAndEvents;

public class StateInfrastructure
{
    public Robot Robot { get; set; }
    public InputJsonDocument InputJsonDocument { get; set; }
    public ResultJsonDocument ResultJsonDocument { get; set; }
    public ILoggerFactory LoggerFactory { get; set; }
}