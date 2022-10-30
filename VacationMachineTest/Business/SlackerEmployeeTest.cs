using NSubstitute;
using NUnit.Framework;
using VacationMachine;
using VacationMachine.Business;

namespace VacationMachineTest
{
    public class SlackerEmployeeTest
    {
        private IEmailSender _emailSender;
        private SlackerEmployee _sut;

        [SetUp]
        public void Initialize()
        {
            _emailSender = Substitute.For<IEmailSender>();

            _sut = new SlackerEmployee(_emailSender);
        }

        [Test]
        [TestCase(1)]
        [TestCase(Configuration.MAX_DAYS)]
        [TestCase(Configuration.MAX_DAYS + 1)]
        [TestCase(Configuration.MAX_DAYS_FOR_PERFORMERS + 1)]
        public void RequestPaidDaysOff_WhenAnyDaysRequested_ThenDenied(int days)
        {
            var expectedResult = typeof(DeniedRequestResult);

            var actualResult = _sut.RequestPaidDaysOff(days).GetType();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
