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
          Default  = 26,
          Slacker = 0
        },
        Additional = new()
        {
            Default = 0,
            Performer = 19
        }
    };
    
    private VacationService _sut = null!;
    
    [SetUp]
    public void Setup()
    {
        Mock<IOptions<VacationDaysLimitSettings>> optionsMock = new();
        optionsMock.Setup(a => a.Current)
            .Returns(_settings);
        
        _sut = new VacationService(_database,
            Array.Empty<IResultHandler>(), optionsMock.Object);
    }

    [TestCase(Employee.EmployeeStatus.Slacker, 0, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Regular, 24, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Regular, 25, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Regular, 26, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Regular, 0, 27, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Performer, 24, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Performer, 25, 1, Result.Approved)]
    [TestCase(Employee.EmployeeStatus.Performer, 26, 1, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 0, 27, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 44, 1, Result.Manual)]
    [TestCase(Employee.EmployeeStatus.Performer, 45, 1, Result.Denied)]
    [TestCase(Employee.EmployeeStatus.Performer, 0, 46, Result.Denied)]
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