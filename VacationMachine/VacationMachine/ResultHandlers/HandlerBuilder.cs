using System;
using System.Collections.Generic;
using VacationMachine.IoC;

namespace VacationMachine.ResultHandlers;

public class HandlerBuilder<THandler> : IHandlerBuilder<THandler>
{
    private readonly IoCProvider _ioCProvider;
    private readonly List<Type> _registeredTypes = new();

    public HandlerBuilder(IoCProvider ioCProvider)
    {
        _ioCProvider = ioCProvider;
    }

    public IHandlerBuilder<THandler> RegisterHandler<T>() where T : THandler
    {
        _registeredTypes.Add(typeof(T));
        return this;
    }

    public IEnumerable<THandler> Build()
    {
        foreach (Type registeredType in _registeredTypes)
        {
            yield return (THandler)_ioCProvider.Get(registeredType);
        }
    }
}