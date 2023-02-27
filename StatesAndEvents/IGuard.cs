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
    public abstract bool Condition(Robot robot);

    public abstract uint Priority { get; }
    public const uint RobotPhase = 1;
}

public interface IGuard<TFinalState> where TFinalState : BaseState
{
    public abstract bool Condition(Robot robot);

    public const uint RobotPhase = 1;
}