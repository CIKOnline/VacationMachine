namespace VacationMachine
{
    public class RegularEmployee : Employee
    {
        public RegularEmployee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender
        ) : base(vacationDatabase, messageBus, emailSender)
        {
        }

        public override Result RequestPaidDaysOff(int days)
        {
            if (DaysSoFar + days <= 26)
            {
                ApprovePaidDaysOff(days);
                return Result.Approved;
            }
            DeniePaidDaysOff();
            return Result.Denied;
        }
    }
}
