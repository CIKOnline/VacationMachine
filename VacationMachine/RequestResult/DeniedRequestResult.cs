using VacationMachine.Business;

namespace VacationMachine
{
    public class DeniedRequestResult : IRequestResult
    {
        private readonly IEmailSender _emailSender;

        public DeniedRequestResult(
            Employee employe,
            IEmailSender emailSender
        )
        {
            Employee = employe;
            _emailSender = emailSender;
        }

        public string StatusMessage => Configuration.STATUS_MESSAGE_DENIED;

        public Employee Employee { get; private set; }

        public bool IsEmployeeChanged => false;

        public void ProcessRequest()
        {
            _emailSender.Send("next time");
        }
    }
}
