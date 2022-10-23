using VacationMachine.AppSettings;
using VacationMachine.Database.Schema;
using VacationMachine.Email;

namespace VacationMachine.ResultHandlers;

public class SendEmailResultResultHandler : IResultHandler
{
    private readonly IEmailSender _emailSender;
    private readonly IOptions<EmailMessageSetting> _emailOptions;

    public SendEmailResultResultHandler(IEmailSender emailSender, IOptions<EmailMessageSetting> emailOptions)
    {
        _emailSender = emailSender;
        _emailOptions = emailOptions;
    }
    
    public void Handle(Result result, Employee employee, long daysToTake)
    {
        var message =
            _emailOptions.Current.GetType()
                .GetProperty(result.ToString())
                ?.GetValue(result) as string;
        
        if(message is not null)
            _emailSender.Send(message);
    }
}