using ThirdPartyLibraries.Email;
using ThirdPartyLibraries.Escalation;
using ThirdPartyLibraries.MessageBus;
using VacationMachine.AppSettings;
using VacationMachine.BusinessLogic.ResultHandlers.Concrete;
using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.BusinessLogic.Services.Concrete;
using VacationMachine.BusinessLogic.Services.Interfaces;
using VacationMachine.DataAccess.Repositories.Concrete;
using VacationMachine.DataAccess.Repositories.Interfaces;
using VacationMachine.IoC;

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

        ioCContainer.RegisterSingleton<IVacationRepository, VacationRepository>();
        ioCContainer.RegisterTransient<IVacationCalculator, VacationCalculator>();
        ioCContainer.RegisterTransient<IEmailSender, EmailSender>();
        ioCContainer.RegisterTransient<IEscalationManager, EscalationManager>();
        ioCContainer.RegisterTransient<IMessageBus, MessageBus>();
        ioCContainer.RegisterTransient<IVacationRequestProcessorService, VacationRequestProcessorService>();

        return ioCContainer;
    }
}