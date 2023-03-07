using Appccelerate.StateMachine;
using JsonDocumentsManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using Serilog;
using TheRobot.Requests;
using TheRobot.MediatedRequests;

namespace StatesAndEvents;

/// <summary>
/// This is the BaseState of the state machine.
/// It's has three protected properties:
/// _robot = It's an instance of a selenium robot...
/// _orcamento = It's an instance of the input data
/// _results = It's an instance of the output data
/// </summary>
public class BaseState : IState
{
    public string Name { get; private set; }

    public virtual TimeSpan StateTimeout => TimeSpan.FromSeconds(5);

    protected readonly Robot _robot;
    protected readonly InputJsonDocument _inputData;
    protected readonly ResultJsonDocument _results;

    public BaseState(string name, Robot robot, InputJsonDocument inputdata, ResultJsonDocument resultJson)
    {
        Name = name;
        _robot = robot;
        _inputData = inputdata;
        _results = resultJson;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((BaseState)obj);
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode();
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    protected bool Equals(BaseState other)
    {
        return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public virtual Task Execute(CancellationToken token)
    {
        return Task.CompletedTask;
    }
}