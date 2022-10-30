using NSubstitute;
using NUnit.Framework;
using VacationMachine;
using VacationMachine.Business;

namespace VacationMachineTest.Employee
{
    public class PerformerEmployeeTest
    {
        private IMessageBus _messageBus;
        private IEmailSender _emailSender;
        private IEscalationManager _escalationManager;
        private PerformerEmployee _sut;

        [SetUp]
        public void Initialize()
        {
            _messageBus = Substitute.For<IMessageBus>();
            _emailSender = Substitute.For<IEmailSender>();
            _escalationManager = Substitute.For<IEscalationManager>();

            _sut = new PerformerEmployee(_messageBus, _emailSender, _escalationManager);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseHelper), nameof(TestCaseHelper.GetDaysFromTo), new object[] { 1, Configuration.MAX_DAYS })]
        public void RequestPaidDaysOff_WhenDaysBelowMaxDaysRequested_ThenApproved(int days)
        {
            var expectedResult = typeof(ApprovedVacationRequest);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseHelper), nameof(TestCaseHelper.GetDaysFromTo), new object[] { Configuration.MAX_DAYS + 1, Configuration.MAX_DAYS_FOR_PERFORMERS })]
        public void RequestPaidDaysOff_WhenDaysAboveMaxDaysAndBelowMaxDaysForPerformesRequested_ThenManual(int days)
        {
            var expectedResult = typeof(ManualVacationRequest);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(Configuration.MAX_DAYS_FOR_PERFORMERS + 1)]
        public void RequestPaidDaysOff_WhenDaysAboveMaxDaysForPerformesRequested_ThenDenied(int days)
        {
            var expectedResult = typeof(DeniedVacationRequest);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
