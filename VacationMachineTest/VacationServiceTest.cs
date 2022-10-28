using NSubstitute;
using NUnit.Framework;
using System;
using VacationMachine;

namespace VacationMachineTest
{
    [SetUpFixture]
    public class VacationServiceTest
    {
        private const int EMPLOYEE_ID = 1;

        private VacationService _sut;
        private IVacationDatabase _vacationDatabase;
        private IMessageBus _messageBus;
        private IEmailSender _emailSender;
        private IEscalationManager _escalationManager;

        [SetUp]
        public void Initialize()
        {
            _vacationDatabase = Substitute.For<IVacationDatabase>();
            _messageBus = Substitute.For<IMessageBus>();
            _emailSender = Substitute.For<IEmailSender>();
            _escalationManager = Substitute.For<IEscalationManager>();

            _sut = new VacationService(
                _vacationDatabase,
                _messageBus,
                _emailSender,
                _escalationManager
            );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void RequestPaidDaysOff_WhenDaysToLow_ThenArgumentException(int days)
        {
            var expectedMessage = $"Invalid amount of days: {days}";

            var exception = Assert.Throws<ArgumentException>(() => _sut.RequestPaidDaysOff(days, EMPLOYEE_ID));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        public void RequestPaidDaysOff_WhenPerformerAndDaysBelowAcceptanceRequirementRange_ThenApproved()
        {
            var expectedResult = Result.Approved;

            _vacationDatabase.FindByEmployeeId(EMPLOYEE_ID).Returns(callInfo =>
            {
                return new Employee
                {
                    Status = "PERFORMER",
                    DaysSoFar = 0
                };
            });

            for (var days = 1; days <= 26; days++)
            {
                RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days);
            }
        }

        [Test]
        public void RequestPaidDaysOff_WhenPerformerAndDaysInAcceptanceRequirementRange_ThenManual()
        {
            var expectedResult = Result.Manual;

            _vacationDatabase.FindByEmployeeId(EMPLOYEE_ID).Returns(callInfo =>
            {
                return new Employee
                {
                    Status = "PERFORMER",
                    DaysSoFar = 0
                };
            });

            for (var days = 27; days < 45; days++)
            {
                RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days);
                _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            }
        }

        [Test]
        public void RequestPaidDaysOff_WhenPerformerAndDaysAboveAcceptanceRequirementRange_ThenDenied()
        {
            var expectedResult = Result.Denied;

            _vacationDatabase.FindByEmployeeId(EMPLOYEE_ID).Returns(callInfo =>
            {
                return new Employee
                {
                    Status = "PERFORMER",
                    DaysSoFar = 0
                };
            });

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, 45);
            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, 46);
        }

        private void RequestPaidDaysOff_ReturnsExpectedResultForDays(Result expectedResult, int days)
        {
            var actualResult = _sut.RequestPaidDaysOff(days, EMPLOYEE_ID);

            Assert.AreEqual(expectedResult, actualResult, $"Test failed for days = {days}");
        }
    }
}
