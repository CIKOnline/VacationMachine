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
        [TestCaseSource(nameof(GetDaysFromTo), new object[] { 1, 26 })]
        public void RequestPaidDaysOff_WhenPerformerAndDaysBelowAcceptanceRequirementRange_ThenApproved(int days)
        {
            var expectedResult = Result.Approved;

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days);
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

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days);
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

            RequestPaidDaysOff_ReturnsExpectedResultForDays(expectedResult, days);
            _vacationDatabase.DidNotReceiveWithAnyArgs().Save(default);
            _messageBus.DidNotReceiveWithAnyArgs().SendEvent(default);
            _emailSender.Received(1).Send("next time");
            _escalationManager.DidNotReceiveWithAnyArgs().NotifyNewPendingRequest(default);
        }

        private void RequestPaidDaysOff_ReturnsExpectedResultForDays(Result expectedResult, int days)
        {
            _vacationDatabase.FindByEmployeeId(EMPLOYEE_ID).Returns(callInfo =>
            {
                return new Employee
                {
                    Status = "PERFORMER",
                    DaysSoFar = 0
                };
            });

            var actualResult = _sut.RequestPaidDaysOff(days, EMPLOYEE_ID);

            Assert.AreEqual(expectedResult, actualResult);
        }

        private static IEnumerable<int> GetDaysFromTo(int firstDay, int lastDay)
        {
            for (var days = firstDay; days <= lastDay; days++)
            {
                yield return days;
            }
        }
    }
}
