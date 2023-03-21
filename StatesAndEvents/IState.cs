namespace StatesAndEvents;

public interface IState : IComparable
{
    public abstract TimeSpan StateTimeout { get; }
}