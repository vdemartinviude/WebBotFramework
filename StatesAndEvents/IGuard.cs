using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace StatesAndEvents;

public enum MachineEvents
{
    NormalTransition,
    AbortTransition,
    FinalizeMachine
}

public interface IGuard<TCurrentState, TNextState> where TCurrentState : BaseState where TNextState : BaseState
{
    public abstract Task<bool> Condition(Robot robot, CancellationToken token);

    public abstract uint Priority { get; }
}

public interface IGuard<TFinalState> where TFinalState : BaseState
{
    public abstract Task<bool> Condition(Robot robot, CancellationToken token);
}