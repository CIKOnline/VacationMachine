using VacationMachine.Enums;
using VacationMachine.Interfaces;
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

        public void Handle(long employeeId, Result result, int daysToTake)
        {
            if (result == Result.Approved)
            {
                _vacationDatabase.AddEmployeeHolidays(employeeId, daysToTake);
            }            
        }
    }
}
