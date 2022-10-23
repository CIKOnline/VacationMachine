using VacationMachine.AppSettings;
using VacationMachine.Database;
using VacationMachine.Email;
using VacationMachine.Escalation;
using VacationMachine.IoC;
using VacationMachine.MessageBus;
using VacationMachine.ResultHandlers;

namespace VacationMachine;

public static class IocProviderConfiguration
{
    public static IoCContainer Configure()
    {
        IoCContainer ioCContainer = new();
        ioCContainer.RegisterSingleton<IAppSettingsReader, AppSettingsReader>();
        ioCContainer.RegisterTransient<IOptions<VacationDaysLimitSettings>, GenericOptions<VacationDaysLimitSettings>>();
        ioCContainer.RegisterTransient<IOptions<MessageBusMessageSettings>, GenericOptions<MessageBusMessageSettings>>();
        ioCContainer.RegisterTransient<IOptions<EmailMessageSettings>, GenericOptions<EmailMessageSettings>>();

        ioCContainer.RegisterTransient<IResultHandler, EscalationManagerResultHandler>();
        ioCContainer.RegisterTransient<IResultHandler, MessageBusResultHandler>();
        ioCContainer.RegisterTransient<IResultHandler, SendEmailResultResultHandler>();
        ioCContainer.RegisterTransient<IResultHandler, UpdateDataInDatabaseResultHandler>();

        ioCContainer.RegisterSingleton<IVacationDatabase, VacationDatabase>();
        ioCContainer.RegisterTransient<IEmailSender, EmailSender>();
        ioCContainer.RegisterTransient<IEscalationManager, EscalationManager>();
        ioCContainer.RegisterTransient<IMessageBus, MessageBus.MessageBus>();
        ioCContainer.RegisterTransient<VacationService>();

        return ioCContainer;
    }
}