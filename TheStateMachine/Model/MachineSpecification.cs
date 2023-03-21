namespace TheStateMachine.Model;

public class MachineSpecification
{
    public IEnumerable<Type>? States { get; set; }
    public IEnumerable<IntermediaryGuard>? IntermediaryGuards { get; set; }
    public IEnumerable<FinalGuard>? FinalGuards { get; set; }
}

public class IntermediaryGuard
{
    public Type? Guard { get; set; }
    public Type? CurrentState { get; set; }
    public Type? NextState { get; set; }
    public string? Namespace { get; set; }
}

public class FinalGuard
{
    public Type? Guard { get; set; }
    public Type? CurrentState { get; set; }
}