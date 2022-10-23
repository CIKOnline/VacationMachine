using System;
using System.Collections.Generic;
using VacationMachine.Database;
using VacationMachine.Database.Schema;
using VacationMachine.IoC;
using VacationMachine.Models;

namespace VacationMachine;

public class Program
{
    private static IoCProvider _ioCProvider = null!;

    private static void Main(string[] args)
    {
        Configure();
        LoadData();
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
        _ioCProvider = IocProviderConfiguration.Configure().BuildProvider();
    }

    private static void LoadData()
    {
        _ioCProvider.Get<IVacationDatabase>().Save(new Employee
        {
            Status = Employee.EmployeeStatus.Regular,
            EmployeeId = 1,
            ApprovedVacationRequestsList = new List<ApprovedVacationRequests>
            {
                new () { Days = 7 }
            }
        }); 
    }
}