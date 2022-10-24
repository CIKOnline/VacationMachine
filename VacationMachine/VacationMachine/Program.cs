using System;
using VacationMachine.BusinessLogic.Services.Interfaces;
using VacationMachine.DataAccess.DataModels;
using VacationMachine.DataAccess.DataModels.Enums;
using VacationMachine.DataAccess.Repositories.Interfaces;
using VacationMachine.IoC;
using VacationMachine.Models;

namespace VacationMachine;

public class Program
{
    private static IoCProvider _ioCProvider = null!;

    private static void Main()
    {
        ConfigureApp();
        LoadData();
        var vacationService = _ioCProvider.Get<IVacationRequestProcessorService>();
        Result result = vacationService.RequestPaidDaysOff(new RequestModel
        {
            EmployeeId = 1,
            DaysToTake = 1
        });

        Console.WriteLine($"Vacation is: {result}");
        Console.ReadKey();
    }

    private static void ConfigureApp()
    {
        _ioCProvider = IocProviderConfiguration.Configure().BuildProvider();
    }

    private static void LoadData()
    {
        _ioCProvider.Get<IVacationRepository>().Save(new Employee
        {
            Status = EmployeeStatus.Slacker,
            EmployeeId = 1
        });
    }
}