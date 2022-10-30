using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public class PerformerEmployee : Employee
    {
        public override EmployeeStatus Status => EmployeeStatus.Performer;

        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public PerformerEmployee(
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        )
        {
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            var newDaysSoFar = DaysSoFar + days;
            if (newDaysSoFar <= Configuration.MAX_DAYS)
            {
                return new ApprovedRequestResult(this, _messageBus, days);
            }
            else if (newDaysSoFar <= Configuration.MAX_DAYS_FOR_PERFORMERS)
            {
                return new ManualRequestResult(this, _escalationManager);
            }
            return new DeniedRequestResult(this, _emailSender);
        }
    }
}
