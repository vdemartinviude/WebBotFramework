using OpenQA.Selenium;

namespace StatesAndEvents;

public interface IEvent : IComparable
{
    public By? By { get; set; }

    public bool ReadyToFire();
}