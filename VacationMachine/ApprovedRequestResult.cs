namespace VacationMachine
{
    public class ApprovedRequestResult : IRequestResult
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly Employee _employee;
        private readonly int days;

        public ApprovedRequestResult(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            Employee employee,
            int days
        )
        {
            _vacationDatabase = vacationDatabase;
            _messageBus = messageBus;
            _employee = employee;
            this.days = days;
        }

        public string Name => "Approved";

        public void ProcessRequest()
        {
            _employee.DaysSoFar += days;
            _vacationDatabase.Save(_employee);
            _messageBus.SendEvent("request approved");
        }
    }
}
