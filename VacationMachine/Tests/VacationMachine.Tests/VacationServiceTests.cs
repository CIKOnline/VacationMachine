using Moq;
using VacationMachine.AppSettings;
using VacationMachine.Database;
using VacationMachine.Database.Schema;
using VacationMachine.Models;
using VacationMachine.ResultHandlers;

namespace VacationMachine.Tests;

public class VacationServiceTests
{
    private readonly IVacationDatabase _database = new VacationDatabase();
    private readonly VacationDaysLimitSettings _settings = new()
    {
        Default = new()
        {
          Default  = 28,
          Slacker = 0
        },
        Additional = new()
        {
            Default = 0,
            Performer = 18
        }
    };
    
    private VacationService _sut = null!;
    
    [SetUp]
    public void Setup()
    {
        Mock<IAppSettingsReader> appSettingsReader = new();
        appSettingsReader.Setup(
                a => a.Read<VacationDaysLimitSettings>())
            .Returns(_settings);

        var options = new GenericOptions<VacationDaysLimitSettings>(appSettingsReader.Object);
        
        _sut = new VacationService(_database,
            Array.Empty<IResultHandler>(), options);
    }

    [TestCase(Employee.EmployeeStatus.Slacker, 0, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Regular, 26, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Regular, 27, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Regular, 28, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Regular, 0, 29, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Performer, 26, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Performer, 27, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Performer, 28, 1, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 0, 29, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 45, 1, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 46, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Performer, 0, 47, Result.Denied)]
    public void CheckExpectedResult(Employee.EmployeeStatus employeeStatus, long daysAlreadyTaken, long daysRequested,
        Result expectedResult)
    {
        //Arrange
        _database.Save(new Employee
        {
            Status = employeeStatus,
            EmployeeId = 1,
            ApprovedVacationRequestsList = new List<ApprovedVacationRequests>
            {
                new()
                {
                    Days = daysAlreadyTaken
                }
            }
        });
        
        //Act
        Result actualResult = _sut.RequestPaidDaysOff(new RequestModel { EmployeeId = 1, DaysToTake = daysRequested });

        //Assert
        Assert.That(expectedResult, Is.EqualTo(actualResult));
    }
}