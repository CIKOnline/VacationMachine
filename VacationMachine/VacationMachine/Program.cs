﻿using System;
using VacationMachine.AppSettings;
using VacationMachine.Database;
using VacationMachine.Database.Schema;
using VacationMachine.Email;
using VacationMachine.Escalation;
using VacationMachine.IoC;
using VacationMachine.MessageBus;
using VacationMachine.Models;
using VacationMachine.ResultHandlers;

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

        Console.WriteLine($"Vacation is: {result}");
        Console.ReadKey();
    }

    private static void Configure()
    {
        IoCContainer ioCContainer = new();
        ioCContainer.RegisterSingleton<AppSettingsReader>();
        ioCContainer.RegisterTransient<IOptions<VacationDaysLimitSettings>, GenericOptions<VacationDaysLimitSettings>>();
        ioCContainer.RegisterTransient<IOptions<MessageBusMessageSettings>, GenericOptions<MessageBusMessageSettings>>();
        ioCContainer.RegisterTransient<IOptions<EmailMessageSetting>, GenericOptions<EmailMessageSetting>>();
        
        ioCContainer.RegisterTransient<EscalationManagerHandler>();
        ioCContainer.RegisterTransient<MessageBusResultHandler>();
        ioCContainer.RegisterTransient<SendEmailResultResultHandler>();
        ioCContainer.RegisterTransient<UpdateDataInDatabaseResultHandler>();
        ioCContainer.RegisterSingleton<IHandlerBuilder<IResultHandler>, HandlerBuilder<IResultHandler>>();
        
        ioCContainer.RegisterSingleton<IVacationDatabase, VacationDatabase>();
        ioCContainer.RegisterTransient<IEmailSender, EmailSender>();
        ioCContainer.RegisterTransient<IEscalationManager, EscalationManager>();
        ioCContainer.RegisterTransient<IMessageBus, MessageBus.MessageBus>();
        ioCContainer.RegisterTransient<VacationService>();
        
        _ioCProvider = ioCContainer.BuildProvider();

        _ioCProvider.Get<IHandlerBuilder<IResultHandler>>()
            .RegisterHandler<EscalationManagerHandler>()
            .RegisterHandler<MessageBusResultHandler>()
            .RegisterHandler<SendEmailResultResultHandler>()
            .RegisterHandler<UpdateDataInDatabaseResultHandler>();
        
        _ioCProvider.Get<IVacationDatabase>().Save(new Employee
        {
            Status = Employee.EmployeeStatus.Regular,
            EmployeeId = 1,
        });

    }
}