using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatesAndEvents;

public class EventCreator : IEvent
{
    public string EventName { get; private set; }
    public By? By {get; set;}
    public IWebDriver Driver { get; private set; }
    public EventCreator(string eventName,By by,IWebDriver driver)
    {     
        EventName = eventName;
        By = by;
        Driver = driver;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;    
        if (obj.GetType() != GetType()) return false;
        return this.Equals((EventCreator)obj);  

    }
    

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        return this.EventName.GetHashCode();
    }

    protected bool Equals(EventCreator other)
    {
        return this.EventName == other.EventName;
    }

    public bool ReadyToFire()
    {
        try
        {
            var wait = new WebDriverWait(Driver, new TimeSpan(0, 0, 0, 0, 100)).Until(ExpectedConditions.ElementToBeClickable(By));
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (OpenQA.Selenium.WebDriverTimeoutException)
        {
            return false;
        }
        
    }
}
