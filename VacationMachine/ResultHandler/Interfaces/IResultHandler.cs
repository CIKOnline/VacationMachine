using VacationMachine.Enums;
using VacationMachine.Models;

namespace VacationMachine.ResultHandler.Interfaces
{
    public interface IResultHandler
    {
        void Handle(EmployeeModel employee, Result result, int daysToTake);
    }
}
