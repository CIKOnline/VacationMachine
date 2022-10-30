namespace VacationMachine
{
    public class PerformerEmployee : Employee
    {
        private readonly IEscalationManager _escalationManager;

        public PerformerEmployee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        ) : base(vacationDatabase, messageBus, emailSender)
        {
            _escalationManager = escalationManager;
        }

        public override Result RequestPaidDaysOff(int days)
        {
            var newDaysSoFar = DaysSoFar + days;
            if (newDaysSoFar <= 26)
            {
                ApprovePaidDaysOff(days);
                return Result.Approved;
            }
            else if (newDaysSoFar < 45)
            {
                ManualPaidDaysOff();
                return Result.Manual;
            }
            DeniePaidDaysOff();
            return Result.Denied;
        }

        private void ManualPaidDaysOff()
        {
            _escalationManager.NotifyNewPendingRequest(EmployeeId);
        }
    }
}
