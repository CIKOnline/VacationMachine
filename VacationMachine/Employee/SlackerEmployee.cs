using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public class SlackerEmployee : Employee
    {
        public override EmployeeRole Role => EmployeeRole.Slacker;

        private readonly IEmailSender _emailSender;

        public SlackerEmployee(
            IEmailSender emailSender
        )
        {
            _emailSender = emailSender;
        }

        public override IVacationRequest RequestPaidDaysOff(int days)
        {
            return new DeniedVacationRequest(this, _emailSender);
        }
    }
}
