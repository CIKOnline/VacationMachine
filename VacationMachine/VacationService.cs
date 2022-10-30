using System;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;
        private readonly IMapper _mapper;

        public VacationService(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager,
            IMapper mapper)
        {
            _vacationDatabase = vacationDatabase;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
            _mapper = mapper;
        }

        public string RequestPaidDaysOff(int days, long employeeId)
        {
            var domainEmployee = _vacationDatabase.FindByEmployeeId(employeeId);
            var employee = _mapper.ToBusiness(domainEmployee);

            return RequestPaidDaysOff(days, employee);
        }

        protected string RequestPaidDaysOff(int days, Business.Employee employee)
        {
            ValidateRequestedDays(days);

            var requestResult = employee.RequestPaidDaysOff(days);
            requestResult.ProcessRequest();

            return requestResult.Name;
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
