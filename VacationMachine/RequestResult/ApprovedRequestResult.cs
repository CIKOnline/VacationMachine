using VacationMachine.Business;

namespace VacationMachine
{
    public class ApprovedRequestResult : IRequestResult
    {
        private readonly IMessageBus _messageBus;
        private readonly int days;

        public ApprovedRequestResult(
            Employee employee,
            IMessageBus messageBus,
            int days
        )
        {
            Employee = employee;
            _messageBus = messageBus;
            this.days = days;
        }

        public string StatusMessage => Configuration.STATUS_MESSAGE_APPROVED;

        public bool IsEmployeeChanged { get; private set; } = false;

        public Employee Employee { get; private set; }

        public void ProcessRequest()
        {
            Employee.DaysSoFar += days;
            IsEmployeeChanged = true;

            _messageBus.SendEvent("request approved");
        }
    }
}
