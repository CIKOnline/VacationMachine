namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        private readonly IEmailSender _emailSender;

        public VacationDatabase(
            IEmailSender emailSender
        )
        {
            _emailSender = emailSender;
        }

        public Employee FindByEmployeeId(long employeeId)
        {
            return new SlackerEmployee(
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
