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

    protected readonly StateInfrastructure _stateInfra;

    public BaseState(string name, StateInfrastructure stateInfrastructure)
    {
        Name = name;
        _stateInfra = stateInfrastructure;
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