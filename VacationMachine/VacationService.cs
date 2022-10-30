using System;
using VacationMachine.Business;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IEmployeeManager _employeeManager;

        public VacationService(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        public string RequestPaidDaysOff(int days, long employeeId)
        {
            ValidateRequestedDays(days);

            Employee employee = _employeeManager.FindByEmployeeId(employeeId);

            return RequestPaidDaysOff(days, employee);
        }

        private string RequestPaidDaysOff(int days, Employee employee)
        {
            var vacationRequest = employee.RequestPaidDaysOff(days);

            vacationRequest.ProcessRequest();

            if (vacationRequest.IsEmployeeUpdated)
            {
                _employeeManager.SaveEmployee(vacationRequest.Employee);
            }

            return vacationRequest.StatusMessage;
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
