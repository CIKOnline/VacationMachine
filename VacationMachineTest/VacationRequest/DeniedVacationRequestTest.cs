using NSubstitute;
using NUnit.Framework;
using VacationMachine;

namespace VacationMachineTest.RequestResult
{
    public class DeniedVacationRequestTest
    {
        private VacationMachine.Business.Employee _employee;
        private IEmailSender _emailSender;
        private DeniedVacationRequest _sut;

        [SetUp]
        public void Initialize()
        {
            _employee = Substitute.For<VacationMachine.Business.Employee>();
            _emailSender = Substitute.For<IEmailSender>();

            _sut = new DeniedVacationRequest(_employee, _emailSender);
        }

        [Test]
        public void ProcessRequest_ThenEmailSenderSendCalled()
        {
            _sut.ProcessRequest();

            _emailSender.Received(1).Send("next time");
        }
    }
}
