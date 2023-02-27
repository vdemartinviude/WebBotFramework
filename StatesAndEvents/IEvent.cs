using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatesAndEvents;

public interface IEvent : IComparable
{
    public By? By {get; set;}
    public bool ReadyToFire();
}
