using VacationMachine.Business;

namespace VacationMachine
{
    public class ManualVacationRequest : IVacationRequest
    {
        private readonly IEscalationManager _escalationManager;

        public string StatusMessage => Configuration.STATUS_MESSAGE_MANUAL;
        public Employee Employee { get; private set; }
        public bool IsEmployeeUpdated => false;

        public ManualVacationRequest(
            Employee employee,
            IEscalationManager escalationManager
        )
        {
            Employee = employee;
            _escalationManager = escalationManager;
        }

        public void ProcessRequest()
        {
            _escalationManager.NotifyNewPendingRequest(Employee.EmployeeId);
        }
    }
}
