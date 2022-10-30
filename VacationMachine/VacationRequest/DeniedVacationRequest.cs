using VacationMachine.Business;

namespace VacationMachine
{
    public class DeniedVacationRequest : IVacationRequest
    {
        private readonly IEmailSender _emailSender;

        public string StatusMessage => Configuration.STATUS_MESSAGE_DENIED;
        public Employee Employee { get; private set; }

        public bool IsEmployeeUpdated => false;

        public DeniedVacationRequest(
            Employee employe,
            IEmailSender emailSender
        )
        {
            Employee = employe;
            _emailSender = emailSender;
        }

        public void ProcessRequest()
        {
            _emailSender.Send("next time");
        }
    }
}
