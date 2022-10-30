using System;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IEmployeeMapper _mapper;

        public VacationService(
            IVacationDatabase vacationDatabase,
            IEmployeeMapper mapper
        )
        {
            _vacationDatabase = vacationDatabase;
            _mapper = mapper;
        }

        public string RequestPaidDaysOff(int days, long employeeId)
        {
            ValidateRequestedDays(days);

            lock (_vacationDatabase)
            {
                var domainEmployee = _vacationDatabase.FindByEmployeeId(employeeId);
                var employee = _mapper.ToBusiness(domainEmployee);

                return RequestPaidDaysOff(days, employee);
            }
        }

        protected string RequestPaidDaysOff(int days, Business.Employee employee)
        {
            var requestResult = employee.RequestPaidDaysOff(days);
            requestResult.ProcessRequest();

            if (requestResult.IsEmployeeChanged)
            {
                var domainEmployee = _mapper.ToDomain(requestResult.Employee);
                _vacationDatabase.Save(domainEmployee);
            }

            return requestResult.StatusMessage;
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
