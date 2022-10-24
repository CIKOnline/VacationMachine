using Moq;
using VacationMachine.AppSettings;
using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.BusinessLogic.Services.Concrete;
using VacationMachine.DataAccess.DataModels;
using VacationMachine.DataAccess.DataModels.Enums;
using VacationMachine.DataAccess.Repositories.Concrete;
using VacationMachine.DataAccess.Repositories.Interfaces;
using VacationMachine.Models;

namespace VacationMachine.Tests;

public class VacationServiceTests
{
    private readonly IVacationRepository _repository = new VacationRepository();

    private readonly VacationDaysLimitSettings _settings = new()
    {
        Default = new VacationDaysLimitSettings.DefaultValues
        {
            Default = 26,
            Slacker = 0
        },
        Additional = new VacationDaysLimitSettings.AdditionalValues
        {
            Default = 0,
            Performer = 19
        }
    };

    private VacationRequestProcessorService _sut = null!;

    [SetUp]
    public void Setup()
    {
        Mock<IOptions<VacationDaysLimitSettings>> optionsMock = new();
        optionsMock.Setup(a => a.Current)
            .Returns(_settings);

        VacationCalculator vacationCalculator = new();
        _sut = new VacationRequestProcessorService(_repository, vacationCalculator, Array.Empty<IResultHandler>(),
            optionsMock.Object);
    }

    [TestCase(EmployeeStatus.Slacker, 0, 1, Result.Denied)]
    [TestCase(EmployeeStatus.Regular, 24, 1, Result.Approved)]
    [TestCase(EmployeeStatus.Regular, 25, 1, Result.Approved)]
    [TestCase(EmployeeStatus.Regular, 26, 1, Result.Denied)]
    [TestCase(EmployeeStatus.Regular, 0, 27, Result.Denied)]
    [TestCase(EmployeeStatus.Performer, 24, 1, Result.Approved)]
    [TestCase(EmployeeStatus.Performer, 25, 1, Result.Approved)]
    [TestCase(EmployeeStatus.Performer, 26, 1, Result.Manual)]
    [TestCase(EmployeeStatus.Performer, 0, 27, Result.Manual)]
    [TestCase(EmployeeStatus.Performer, 44, 1, Result.Manual)]
    [TestCase(EmployeeStatus.Performer, 45, 1, Result.Denied)]
    [TestCase(EmployeeStatus.Performer, 0, 46, Result.Denied)]
    public void CheckExpectedResult(EmployeeStatus employeeStatus, long daysAlreadyTaken, long daysRequested,
        Result expectedResult)
    {
        //Arrange
        _repository.Save(new Employee
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