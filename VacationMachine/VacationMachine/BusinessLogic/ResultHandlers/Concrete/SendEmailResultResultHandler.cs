using ThirdPartyLibraries.Email;
using VacationMachine.AppSettings;
using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.DataAccess.DataModels;

namespace VacationMachine.BusinessLogic.ResultHandlers.Concrete;

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