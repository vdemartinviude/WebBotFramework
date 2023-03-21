using JsonDocumentsManager;
using Microsoft.Extensions.Logging;
using TheRobot;

namespace StatesAndEvents;

public class StateInfrastructure
{
    public Robot Robot { get; set; }
    public InputJsonDocument InputJsonDocument { get; set; }
    public ResultJsonDocument ResultJsonDocument { get; set; }
    public ILoggerFactory LoggerFactory { get; set; }
}