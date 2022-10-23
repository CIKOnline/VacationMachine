using System.Collections;
using System.Reflection;
using VacationMachine.IoC.Enums;

namespace VacationMachine.IoC;

public class IoCProvider
{
    private readonly ILookup<Type, ImplementationInstruction> _resolvingInstructions;

    internal IoCProvider(List<(Type, ImplementationInstruction)> implementationMappings)
    {
        _resolvingInstructions = implementationMappings.ToLookup(
            p => p.Item1,
            p => p.Item2);
    }

    public T Get<T>()
    {
        Type type = typeof(T);
        return (T)Get(type);
    }

    public IEnumerable<T> GetAll<T>()
    {
        Type type = typeof(T);
        return (IEnumerable<T>)GetAll(type);
    }

    public object Get(Type type)
    {
        return Get(type, new List<Type>()).First();
    }

    public IEnumerable<object> GetAll(Type type)
    {
        return Get(type, new List<Type>());
    }

    private IEnumerable<object> Get(Type type, List<Type> blockedTypes)
    {
        if (type == GetType())
        {
            yield return this;

            yield break;
        }

        if (blockedTypes.Contains(type))
            throw new Exception("Circular dependency was detected");

        var resolvingInstructions = GetResolvingInstructions(type);

        foreach (ImplementationInstruction implementationInstruction in resolvingInstructions)
        {
            if (implementationInstruction.DependencyType == DependencyType.Singleton &&
                implementationInstruction.Cached is not null)
                yield return implementationInstruction.Cached;

            blockedTypes.Add(type);

            object resolvedObject = ResolveType(implementationInstruction.Type, blockedTypes);
            if (implementationInstruction.DependencyType == DependencyType.Singleton)
                implementationInstruction.Cached = resolvedObject;

            blockedTypes.Remove(type);

            yield return resolvedObject;
        }
    }

    private IEnumerable<ImplementationInstruction> GetResolvingInstructions(Type type)
    {
        if (_resolvingInstructions.Contains(type))
            return _resolvingInstructions[type];

        if (IsIEnumerableOfT(type))
        {
            Type typeToResolve = type.GetGenericArguments().Single();
            if (_resolvingInstructions.Contains(typeToResolve))
                return _resolvingInstructions[typeToResolve];
        }


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
        ConstructorInfo constructor = GetConstructor(type);
        var parameters = constructor.GetParameters();
        List<object?> resolvedParameters = new();
        foreach (ParameterInfo parameter in parameters)
        {
            Type typeToResolve;
            if (IsIEnumerableOfT(parameter.ParameterType))
            {
                //Magic
                typeToResolve = parameter.ParameterType.GetGenericArguments().Single();
                var resolvedTypes = Get(typeToResolve, blockedTypes);
                Type listType = typeof(List<>);
                Type concreteType = listType.MakeGenericType(typeToResolve);
                var list = (IList)Activator.CreateInstance(concreteType)!;

                foreach (object resolvedType in resolvedTypes)
                    list.Add(resolvedType);

                resolvedParameters.Add(list);
            }

            else
            {
                typeToResolve = parameter.ParameterType;
                resolvedParameters.Add(Get(typeToResolve, blockedTypes).First());
            }
        }

        return constructor.Invoke(resolvedParameters.ToArray());
    }

    private static bool IsIEnumerableOfT(Type type)
    {
        return type.GetInterfaces()
            .Append(type)
            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }
}