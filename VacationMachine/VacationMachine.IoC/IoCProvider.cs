using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using VacationMachine.IoC.Enums;

namespace VacationMachine.IoC;

public class IoCProvider
{
    private readonly Dictionary<Type, object> _cachedTypes = new();
    private readonly Dictionary<Type, Type> _implementationMappings;
    private readonly Dictionary<Type, DependencyType> _resolvingStrategyDictionary;

    internal IoCProvider(
        Dictionary<Type, DependencyType> resolvingStrategy,
        Dictionary<Type, Type> implementationMappings)
    {
        _resolvingStrategyDictionary = resolvingStrategy;
        _implementationMappings = implementationMappings;
    }

    public T Get<T>()
    {
        Type type = typeof(T);
        return (T)Get(type, new List<Type>());
    }

    private object Get(Type type, List<Type> blockedTypes)
    {
        if (blockedTypes.Contains(type))
            throw new Exception("Circular dependency was detected");

        bool isSingleton = IsTypeRegisteredAsSingleton(type);
        if (isSingleton && TryToGetAlreadyResolvedObject(type, out object? cachedObject))
            return cachedObject;

        blockedTypes.Add(type);

        object resolvedObject = ResolveType(type, blockedTypes);

        blockedTypes.Remove(type);
        if (isSingleton)
            AddToCache(type, resolvedObject);
        return resolvedObject;
    }

    private Type GetTypeToBeResolved(Type type)
    {
        if (_implementationMappings.ContainsKey(type))
            return _implementationMappings[type];

        throw new Exception($"Type {type.Name} is not registered");
    }

    private ConstructorInfo GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();

        return constructors.Length switch
        {
            0 => throw new Exception($"Type {type.Name} has no public constructor"),
            > 1 => throw new Exception($"Type {type.Name} has more than one public constructor"),
            _ => constructors.Single()
        };
    }

    private object ResolveType(Type type, List<Type> blockedTypes)
    {
        Type target = GetTypeToBeResolved(type);
        ConstructorInfo constructor = GetConstructor(target);
        var parameters = constructor.GetParameters();
        return constructor.Invoke(parameters.Select(item => Get(item.ParameterType, blockedTypes)).ToArray());
    }

    private bool IsTypeRegisteredAsSingleton(Type type)
    {
        return _resolvingStrategyDictionary.ContainsKey(type) && _resolvingStrategyDictionary[type] == DependencyType.Singleton;
    }

    private bool TryToGetAlreadyResolvedObject(Type type, [NotNullWhen(true)] out object? output)
    {
        if (_cachedTypes.ContainsKey(type))
        {
            output = _cachedTypes[type];
            return true;
        }

        output = null;
        return false;
    }

    private void AddToCache(Type type, object resolvedObject)
    {
        _cachedTypes.Add(type, resolvedObject);
    }
}