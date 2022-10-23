using VacationMachine.Database.Schema;

namespace VacationMachine.ResultHandlers;

public interface IResultHandler
{
    void Handle(Result result, Employee employee, long daysToTake);
}