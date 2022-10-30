namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;

        public VacationDatabase(
            IMessageBus messageBus,
            IEmailSender emailSender
        )
        {
            _messageBus = messageBus;
            _emailSender = emailSender;
        }

        public Employee FindByEmployeeId(long employeeId)
        {
            return new SlackerEmployee(
                this,
                _messageBus,
                _emailSender
            )
            {
                EmployeeId = employeeId,
                DaysSoFar = 1
            };
        }

        public void Save(Employee employeeData)
        {
        }
    }
}
