using System.Collections.Generic;

namespace VacationMachine.ResultHandlers;

public interface IHandlerBuilder<THandler>
{
    IHandlerBuilder<THandler> RegisterHandler<T>() where T : THandler;
    IEnumerable<THandler> Build();
}