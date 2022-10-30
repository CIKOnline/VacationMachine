using NSubstitute;
using NUnit.Framework;
using VacationMachine;
using VacationMachine.Business;

namespace VacationMachineTest.RequestResult
{
    public class ManualRequestResultTest
    {
        private Employee _employee;
        private IEscalationManager _esscalationManager;
        private ManualRequestResult _sut;

        [SetUp]
        public void Initialize()
        {
            _employee = Substitute.For<Employee>();
            _esscalationManager = Substitute.For<IEscalationManager>();

            _sut = new ManualRequestResult(_employee, _esscalationManager);
        }

        [Test]
        public void ProcessRequest_ThenEscalationManagerNotifyNewPendingRequestCalled()
        {
            var expectedEmployeeId = 4;
            _employee.EmployeeId = expectedEmployeeId;

            _sut.ProcessRequest();

            _esscalationManager.Received(1).NotifyNewPendingRequest(expectedEmployeeId);
        }
    }
}
