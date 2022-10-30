namespace VacationMachine
{
    public abstract class Employee
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;

        protected Employee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender
        )
        {
            _vacationDatabase = vacationDatabase;
            _messageBus = messageBus;
            _emailSender = emailSender;
        }

        public long EmployeeId { get; set; }
        public int DaysSoFar { get; set; }

        public abstract Result RequestPaidDaysOff(int days);

        protected void DeniePaidDaysOff()
        {
            _emailSender.Send("next time");
        }

        protected void ApprovePaidDaysOff(int days)
        {
            DaysSoFar += days;
            _vacationDatabase.Save(this);
            _messageBus.SendEvent("request approved");
        }
    }
}
