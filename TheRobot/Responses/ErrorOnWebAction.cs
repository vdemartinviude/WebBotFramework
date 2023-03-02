using TheRobot.Responses;

namespace TheRobot.Responses;

public enum SeverityLevel
{
    Expected,
    Warning,
    Critical
}

public class ErrorOnWebAction
{
    public SeverityLevel SeverityLevel;
    public string? Error { get; set; }
}