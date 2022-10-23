using VacationMachine.AppSettings;
using VacationMachine.Database.Schema;
using VacationMachine.Email;

namespace VacationMachine.ResultHandlers;

public class SendEmailResultResultHandler : IResultHandler
{
    private readonly IOptions<EmailMessageSettings> _emailOptions;
    private readonly IEmailSender _emailSender;

    public SendEmailResultResultHandler(IEmailSender emailSender, IOptions<EmailMessageSettings> emailOptions)
    {
        _emailSender = emailSender;
        _emailOptions = emailOptions;
    }

    public void Handle(Result result, Employee employee, long daysToTake)
    {
        var message =
            _emailOptions.Current.GetType()
                .GetProperty(result.ToString())
                ?.GetValue(_emailOptions.Current) as string;

        if (message is not null)
            _emailSender.Send(message);
    }
}