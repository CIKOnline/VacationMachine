using VacationMachine.Enums;
using VacationMachine.Interfaces;
using VacationMachine.Models;
using VacationMachine.ResultHandler.Interfaces;

namespace VacationMachine.ResultHandler
{
    public class VacationDatabaseResultHandler : IResultHandler
    {
        private readonly IVacationDatabase _vacationDatabase;

        public VacationDatabaseResultHandler(IVacationDatabase vacationDatabase)
        {
            _vacationDatabase = vacationDatabase;
        }

        public void Handle(EmployeeModel employee, Result result, int daysToTake)
        {
            if (result == Result.Approved)
            {
                employee.TakenHolidays.Add(daysToTake);
                _vacationDatabase.Save(employee);
            }            
        }
    }
}
