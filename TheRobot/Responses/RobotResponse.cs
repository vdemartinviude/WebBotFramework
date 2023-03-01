using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.Response;

public enum RobotResponseStatus
{
    ActionRealizedOk,
    ElementNotFound,
    ExceptionOccurred,
    TimedOut
}

public class RobotResponse
{
    public RobotResponseStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public IWebElement? WebElement { get; set; }
    public string? Data { get; set; }
    public List<IWebElement>? WebElements { get; set; }
}