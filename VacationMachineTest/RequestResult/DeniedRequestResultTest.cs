using NSubstitute;
using NUnit.Framework;
using VacationMachine;
using VacationMachine.Business;

namespace VacationMachineTest.RequestResult
{
    public class DeniedRequestResultTest
    {
        private Employee _employee;
        private IEmailSender _emailSender;
        private DeniedRequestResult _sut;

        [SetUp]
        public void Initialize()
        {
            _employee = Substitute.For<Employee>();
            _emailSender = Substitute.For<IEmailSender>();

            _sut = new DeniedRequestResult(_employee, _emailSender);
        }

        [Test]
        public void ProcessRequest_ThenEmailSenderSendCalled()
        {
            _sut.ProcessRequest();

            _emailSender.Received(1).Send("next time");
        }
    }
}
