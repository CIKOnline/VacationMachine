using System;
using VacationMachine.AppSettings;
using VacationMachine.Database;
using VacationMachine.Email;
using VacationMachine.Escalation;
using VacationMachine.IoC;
using VacationMachine.MessageBus;
using VacationMachine.Models;

namespace VacationMachine;

public class Program
{
    static IoCProvider _ioCProvider = null!;
    private static void Main(string[] args)
    {
        Configure();
        var vacationService = _ioCProvider.Get<VacationService>();
        Result result = vacationService.RequestPaidDaysOff(new RequestModel
        {
            EmployeeId = 1,
            DaysToTake = 3
        });

        Console.WriteLine($"Vacation is :{result}");
        Console.ReadKey();
    }

    private static void Configure()
    {
        IoCContainer ioCContainer = new();
        ioCContainer.RegisterSingleton<AppSettingsReader>();
        ioCContainer.RegisterTransient<IOptions<VacationDaysLimitSettings>, GenericOptions<VacationDaysLimitSettings>>();
        ioCContainer.RegisterSingleton<IVacationDatabase, VacationDatabase>();
        ioCContainer.RegisterTransient<IEmailSender, EmailSender>();
        ioCContainer.RegisterTransient<IEscalationManager, EscalationManager>();
        ioCContainer.RegisterTransient<IMessageBus, MessageBus.MessageBus>();
        ioCContainer.RegisterTransient<VacationService>();
        _ioCProvider = ioCContainer.BuildProvider();
    }
}