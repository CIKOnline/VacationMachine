using NSubstitute;
using NUnit.Framework;
using VacationMachine;
using VacationMachine.Business;

namespace VacationMachineTest
{
    public class RegularEmployeeTest
    {
        private IMessageBus _messageBus;
        private IEmailSender _emailSender;
        private RegularEmployee _sut;

        [SetUp]
        public void Initialize()
        {
            _messageBus = Substitute.For<IMessageBus>();
            _emailSender = Substitute.For<IEmailSender>();

            _sut = new RegularEmployee(_messageBus, _emailSender);
        }

        [Test]
        [TestCaseSource(typeof(TestCaseHelper), nameof(TestCaseHelper.GetDaysFromTo), new object[] { 1, Configuration.MAX_DAYS })]
        public void RequestPaidDaysOff_WhenDaysBelowMaxDaysRequested_ThenApproved(int days)
        {
            var expectedResult = typeof(ApprovedRequestResult);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(Configuration.MAX_DAYS + 1)]
        [TestCase(Configuration.MAX_DAYS_FOR_PERFORMERS + 1)]
        public void RequestPaidDaysOff_WhenDaysAboveMaxDaysRequested_ThenDenied(int days)
        {
            var expectedResult = typeof(DeniedRequestResult);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
