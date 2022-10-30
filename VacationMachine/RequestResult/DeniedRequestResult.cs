namespace VacationMachine
{
    public class DeniedRequestResult : IRequestResult
    {
        private readonly IEmailSender _emailSender;

        public DeniedRequestResult(
            IEmailSender emailSender
        )
        {
            _emailSender = emailSender;
        }

        public string Name => "Denied";

        public void ProcessRequest()
        {
            _emailSender.Send("next time");
        }
    }
}
