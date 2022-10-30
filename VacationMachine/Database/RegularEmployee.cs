namespace VacationMachine
{
    public class RegularEmployee : Employee
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;

        public RegularEmployee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender
        )
        {
            _vacationDatabase = vacationDatabase;
            _messageBus = messageBus;
            _emailSender = emailSender;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            if (DaysSoFar + days <= 26)
            {
                return new ApprovedRequestResult(_vacationDatabase, _messageBus, this, days);
            }
            return new DeniedRequestResult(_emailSender);
        }
    }
}
