using OneOf;
using OpenQA.Selenium;

namespace TheRobot.MediatedRequests;

public class GenericMediatedParameters
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public TimeSpan TimeOut { get; set; }
    public GenericWebElement ByOrElement { get; set; }
}

public class GenericWebElement : OneOfBase<By, WebElement, RecursiveElement, ElementFromSearchContext, List<WebElement>>
{
    public GenericWebElement(OneOf<By, WebElement, RecursiveElement, ElementFromSearchContext, List<WebElement>> input) : base(input)
    {
    }

    public static implicit operator GenericWebElement(WebElement _) => new(_);
}

public class RecursiveElement
{
    public IWebElement Element { get; set; }
    public By By { get; set; }
}

public class ElementFromSearchContext
{
    public ISearchContext SearchContext { get; set; }
    public string CssSelector { get; set; }
}