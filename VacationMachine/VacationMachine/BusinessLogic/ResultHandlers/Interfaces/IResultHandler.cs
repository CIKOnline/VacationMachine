using VacationMachine.DataAccess.DataModels;

namespace VacationMachine.BusinessLogic.ResultHandlers.Interfaces;

public interface IResultHandler
{
    void Handle(Result result, Employee employee, long daysToTake);
}