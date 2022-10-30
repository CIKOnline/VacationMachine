using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using VacationMachine;

namespace VacationMachineTest
{
    public class VacationServiceTest
    {
        private const int EMPLOYEE_ID = 1;

        private VacationServiceFake _sut;
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

            _sut = new VacationServiceFake(
                _vacationDatabase,
                _messageBus,
                _emailSender,
                _escalationManager
            );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateRequestedDays_WhenDaysToLow_ThenArgumentException(int days)
        {
            var expectedMessage = $"Invalid amount of days: {days}";

            var exception = Assert.Throws<ArgumentException>(() => _sut.ValidateRequestedDays(days));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        [TestCaseSource(nameof(GetDaysFromTo), new object[] { 1, 26 })]
        public void RequestPaidDaysOff_WhenPerformerAndDaysBelowAcceptanceRequirementRange_ThenApproved(int days)
        {
            var expectedResult = Result.Approved;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Performer);
            _vacationDatabase.ReceivedWithAnyArgs(1).Save(default);
            _messageBus.Received(1).SendEvent("request approved");
            _emailSender.DidNotReceiveWithAnyArgs().Send(default);
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        [Test]
        [TestCaseSource(nameof(GetDaysFromTo), new object[] { 27, 44 })]
        public void RequestPaidDaysOff_WhenPerformerAndDaysInAcceptanceRequirementRange_ThenManual(int days)
        {
            var expectedResult = Result.Manual;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Performer);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.DidNotReceiveWithAnyArgs().Send(default);
            _escalationManager.Received(1).NotifyNewPendingRequest(EMPLOYEE_ID);
        }

        [Test]
        [TestCase(45)]
        [TestCase(46)]
        public void RequestPaidDaysOff_WhenPerformerAndDaysAboveAcceptanceRequirementRange_ThenDenied(int days)
        {
            var expectedResult = Result.Denied;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Performer);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        [Test]
        [TestCaseSource(nameof(GetDaysFromTo), new object[] { 1, 26 })]
        public void RequestPaidDaysOff_WhenRegularAndDaysInAcceptedRange_ThenApproved(int days)
        {
            var expectedResult = Result.Approved;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Regular);
            _vacationDatabase.ReceivedWithAnyArgs(1).Save(default);
            _messageBus.Received(1).SendEvent("request approved");
            _emailSender.DidNotReceiveWithAnyArgs().Send(default);
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        [Test]
        [TestCase(27)]
        [TestCase(28)]
        [TestCase(45)]
        [TestCase(46)]
        public void RequestPaidDaysOff_WhenRegularAndDaysAboveAcceptedRange_ThenDenied(int days)
        {
            var expectedResult = Result.Denied;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Regular);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(27)]
        [TestCase(28)]
        [TestCase(45)]
        [TestCase(46)]
        public void RequestPaidDaysOff_WhenSlackerAndAnyDaysRequested_ThenDenied(int days)
        {
            var expectedResult = Result.Denied;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Slacker);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        [Test]
        [TestCase(17, 10)]
        [TestCase(18, 10)]
        [TestCase(1, 30)]
        public void RequestPaidDaysOff_WhenDaysSoFarPlusRequestedDaysToHeigh_ThenDenied(int days, int daysSoFar)
        {
            var expectedResult = Result.Denied;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days, EmployeeStatus.Regular, daysSoFar);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        private void RequestPaidDaysOff_ReturnsExpectedResultForDays(Result expectedResult, int days, EmployeeStatus employeeStatus, int daysSoFar = 0)
        {
            var employee = new Employee
            {
                EmployeeId = EMPLOYEE_ID,
                Status = employeeStatus,
                DaysSoFar = daysSoFar
            };

            var actualResult = _sut.RequestPaidDaysOff(days, employee);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private static IEnumerable<int> GetDaysFromTo(int firstDay, int lastDay)
        {
            for (var days = firstDay; days <= lastDay; days++)
            {
                yield return days;
            }
        }

        private class VacationServiceFake : VacationService
        {
            public VacationServiceFake(
                IVacationDatabase database,
                IMessageBus messageBus,
                IEmailSender emailSender,
                IEscalationManager escalationManager
            ) : base(database, messageBus, emailSender, escalationManager)
            {
            }

            public new Result RequestPaidDaysOff(int days, Employee employee)
            {
                return base.RequestPaidDaysOff(days, employee);
            }

            public new void ValidateRequestedDays(int days)
            {
                base.ValidateRequestedDays(days);
            }
        }
    }
}