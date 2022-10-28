using VacationMachine.Enums;
using VacationMachine.ResultHandler.Interfaces;

namespace VacationMachine.ResultHandler
{
    public class EmailSenderResultHandler : IResultHandler
    {
        private readonly IEmailSender _emailSender;

        public EmailSenderResultHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Handle(long employeeId, Result result, int daysToTake)
        {
            if (result.Equals(Result.Denied))
            {
                _emailSender.Send("next time");
            }
        }
    }
}
