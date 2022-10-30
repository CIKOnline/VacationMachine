using System;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _vacationDatabase;

        public VacationService(
            IVacationDatabase database
        )
        {
            _vacationDatabase = database;
        }

        public Result RequestPaidDaysOff(int days, long employeeId)
        {
            var employee = _vacationDatabase.FindByEmployeeId(employeeId);

            return RequestPaidDaysOff(days, employee);
        }

        protected Result RequestPaidDaysOff(int days, Employee employee)
        {
            ValidateRequestedDays(days);

            return employee.RequestPaidDaysOff(days);
        }

        protected void ValidateRequestedDays(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException($"Invalid amount of days: {days}");
            }
        }
    }
}
