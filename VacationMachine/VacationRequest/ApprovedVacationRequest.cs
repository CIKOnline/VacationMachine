using VacationMachine.Business;

namespace VacationMachine
{
    public class ApprovedVacationRequest : IVacationRequest
    {
        private readonly IMessageBus _messageBus;
        private readonly int _days;

        public string StatusMessage => Configuration.STATUS_MESSAGE_APPROVED;
        public Employee Employee { get; private set; }
        public bool IsEmployeeUpdated { get; private set; } = false;

        public ApprovedVacationRequest(
            Employee employee,
            IMessageBus messageBus,
            int days
        )
        {
            Employee = employee;
            _messageBus = messageBus;
            _days = days;
        }

        public void ProcessRequest()
        {
            Employee.DaysSoFar += _days;
            IsEmployeeUpdated = true;

            _messageBus.SendEvent("request approved");
        }
    }
}
