using VacationMachine.Business;

namespace VacationMachine
{
    public class ManualRequestResult : IRequestResult
    {
        private readonly IEscalationManager _escalationManager;

        public ManualRequestResult(
            Employee employee,
            IEscalationManager escalationManager
        )
        {
            Employee = employee;
            _escalationManager = escalationManager;
        }

        public string StatusMessage => "Manual";

        public Employee Employee { get; private set; }

        public bool IsEmployeeChanged => false;

        public void ProcessRequest()
        {
            _escalationManager.NotifyNewPendingRequest(Employee.EmployeeId);
        }
    }
}
