using VacationMachine.IoC.Enums;

namespace VacationMachine.IoC;

internal class ImplementationInstruction
{
    public Type Type { get; init; } = null!;
    public DependencyType DependencyType { get; init; }
    public object? Cached { get; set; }
}