using VacationMachine.IoC.Enums;

namespace VacationMachine.IoC;

public class IoCContainer
{
    private readonly List<(Type, ImplementationInstruction)> _implementationMappings = new();
    
    public void RegisterSingleton<TImplementation>()
    {
        RegisterSingleton<TImplementation, TImplementation>();
    }
    
    public void RegisterTransient<TImplementation>()
    {
        RegisterTransient<TImplementation, TImplementation>();
    }
    
    public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
    {
        ImplementationInstruction instruction = new()
        {
            Type = typeof(TImplementation),
            DependencyType = DependencyType.Singleton
        };
        _implementationMappings.Add((typeof(TInterface), instruction));
    }

    public void RegisterTransient<TInterface, TImplementation>() where TImplementation : TInterface
    {
        ImplementationInstruction instruction = new()
        {
            Type = typeof(TImplementation),
            DependencyType = DependencyType.Transient
        };
        _implementationMappings.Add((typeof(TInterface), instruction));
    }

    public IoCProvider BuildProvider()
    {
        return new IoCProvider(_implementationMappings);
    }
}