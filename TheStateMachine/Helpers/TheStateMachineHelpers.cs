using StatesAndEvents;
using System.Reflection;
using TheStateMachine.Model;

namespace TheStateMachine.Helpers;

public static class TheStateMachineHelpers
{
    public static MachineSpecification GetMachineSpecification(Assembly assembly)
    {
        var specification = new MachineSpecification();
        specification.States = assembly.ExportedTypes.Where(type => type.BaseType == typeof(BaseState));
        specification.IntermediaryGuards = assembly.ExportedTypes.Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<,>)))
            .Select(ty => new IntermediaryGuard
            {
                Namespace = ty.Namespace!,
                Guard = ty,
                CurrentState = ty.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[0],
                NextState = ty.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<,>)).GenericTypeArguments[1],
            });
        specification.FinalGuards = assembly.ExportedTypes.Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGuard<>)))
            .Select(ty => new FinalGuard
            {
                Guard = ty,
                CurrentState = ty.GetInterfaces().Single(y => y.GetGenericTypeDefinition() == typeof(IGuard<>)).GenericTypeArguments[0]
            });
        return specification;
    }
}