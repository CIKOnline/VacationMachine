namespace VacationMachine
{
    public class SlackerEmployee : Employee
    {
        public SlackerEmployee(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender
        ) : base(vacationDatabase, messageBus, emailSender)
        {
        }

        public override Result RequestPaidDaysOff(int days)
        {
            DeniePaidDaysOff();
            return Result.Denied;
        }
    }
}
