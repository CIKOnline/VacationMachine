using VacationMachine.IoC.Enums;

namespace VacationMachine.IoC;

public class IoCContainer
{
    private readonly Dictionary<Type, Type> _implementationMappings = new();
    private readonly Dictionary<Type, DependencyType> _resolvingStrategy = new();

    public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _resolvingStrategy.Add(typeof(TInterface), DependencyType.Singleton);
        _implementationMappings.Add(typeof(TInterface), typeof(TImplementation));
    }

    public void RegisterTransient<TInterface, TImplementation>() where TImplementation : TInterface
    {
        _resolvingStrategy.Add(typeof(TInterface), DependencyType.Transient);
        _implementationMappings.Add(typeof(TInterface), typeof(TImplementation));
    }

    public IoCProvider BuildProvider()
    {
        return new IoCProvider(_resolvingStrategy, _implementationMappings);
    }
}