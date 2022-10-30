namespace VacationMachine
{
    public class PerformerEmployee : Employee
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public PerformerEmployee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        )
        {
            _vacationDatabase = vacationDatabase;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            var newDaysSoFar = DaysSoFar + days;
            if (newDaysSoFar <= 26)
            {
                return new ApprovedRequestResult(_vacationDatabase, _messageBus, this, days);
            }
            else if (newDaysSoFar < 45)
            {
                return new ManualRequestResult(_escalationManager, EmployeeId);
            }
            return new DeniedRequestResult(_emailSender);
        }
    }
}
