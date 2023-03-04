using OpenQA.Selenium;

namespace TheRobot.Responses;

public class SuccessOnWebAction
{
    public TimeSpan? ElapsedTime { get; set; }
    public string CurrentUrl { get; set; }
    public string CurrentPageTitle { get; set; }
    public IEnumerable<IWebElement> WebElements { get; set; }
    public IWebElement WebElement { get; set; }
}