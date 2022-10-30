using NSubstitute;
using NUnit.Framework;
using VacationMachine;

namespace VacationMachineTest.RequestResult
{
    public class ApprovedVacationRequestTest
    {
        private const int ADDED_DAYS = 5;

        private VacationMachine.Business.Employee _employee;
        private IMessageBus _messageBus;
        private ApprovedVacationRequest _sut;

        [SetUp]
        public void Initialize()
        {
            _employee = Substitute.For<VacationMachine.Business.Employee>();
            _messageBus = Substitute.For<IMessageBus>();

            _sut = new ApprovedVacationRequest(_employee, _messageBus, ADDED_DAYS);
        }

        [Test]
        public void ProcessRequest_ThenDaysAddedAndMessageBusSendEventCalled()
        {
            _employee.DaysSoFar = 3;
            var expectedDaysSoFar = _employee.DaysSoFar + ADDED_DAYS;

            _sut.ProcessRequest();

            Assert.AreEqual(expectedDaysSoFar, _employee.DaysSoFar);
            _messageBus.Received(1).SendEvent("request approved");
        }
    }
}
