using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public class SlackerEmployee : Employee
    {
        public override EmployeeStatus Status => EmployeeStatus.Slacker;

        private readonly IEmailSender _emailSender;

        public SlackerEmployee(
            IEmailSender emailSender
        )
        {
            _emailSender = emailSender;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            return new DeniedRequestResult(_emailSender);
        }
    }
}
