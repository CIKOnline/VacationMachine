using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using VacationMachine;

namespace VacationMachineTest
{
    [TestClass]
    public class VacationServiceTest
    {
        private VacationService _sut;
        private IVacationDatabase _vacationDatabase;
        private IMessageBus _messageBus;
        private IEmailSender _emailSender;
        private IEscalationManager _escalationManager;

        [TestInitialize]
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid amount of days: 0")]
        public void RequestPaidDaysOff_WhenDaysZero_ThenArgumentException()
        {
            _sut.RequestPaidDaysOff(0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid amount of days: -1")]
        public void RequestPaidDaysOff_WhenDaysMinusOne_ThenArgumentException()
        {
            _sut.RequestPaidDaysOff(-1, 1);
        }
    }
}
