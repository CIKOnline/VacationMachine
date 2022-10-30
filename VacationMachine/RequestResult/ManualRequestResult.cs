namespace VacationMachine
{
    public class ManualRequestResult : IRequestResult
    {
        private readonly IEscalationManager _escalationManager;
        private readonly long _employeeId;

        public ManualRequestResult(
            IEscalationManager escalationManager,
            long employeeId
        )
        {
            _escalationManager = escalationManager;
            _employeeId = employeeId;
        }

        public string Name => "Manual";

        public void ProcessRequest()
        {
            _escalationManager.NotifyNewPendingRequest(_employeeId);
        }
    }
}
