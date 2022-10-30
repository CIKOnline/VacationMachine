using NSubstitute;
using NUnit.Framework;
using VacationMachine;

namespace VacationMachineTest.RequestResult
{
    public class ManualVacationRequestTest
    {
        private VacationMachine.Business.Employee _employee;
        private IEscalationManager _esscalationManager;
        private ManualVacationRequest _sut;

        [SetUp]
        public void Initialize()
        {
            _employee = Substitute.For<VacationMachine.Business.Employee>();
            _esscalationManager = Substitute.For<IEscalationManager>();

            _sut = new ManualVacationRequest(_employee, _esscalationManager);
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
