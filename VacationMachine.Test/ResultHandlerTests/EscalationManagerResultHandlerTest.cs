using Moq;
using VacationMachine.Enums;
using VacationMachine.ResultHandler;

namespace VacationMachine.Test.ResultHandlerTests
{
    [TestClass]
    public class EscalationManagerResultHandlerTest : ResultHandlerBaseTest
    {
        [TestMethod]
        public void Handle_WhenResultApproved_ThenNotifyNewPendingRequestNotCalled()
        {
            // Arrange
            var escalationManagerMock = new Mock<IEscalationManager>();
            var escalationManagerResultHandler = new EscalationManagerResultHandler(escalationManagerMock.Object);

            // Act
            escalationManagerResultHandler.Handle(GetDefaultEmployee(), Result.Approved, 0);

            // Assert
            escalationManagerMock.Verify(v => v.NotifyNewPendingRequest(1), Times.Never());
        }

        [TestMethod]
        public void Handle_WhenResultDenied_ThenNotifyNewPendingRequestNotCalled()
        {
            // Arrange
            var escalationManagerMock = new Mock<IEscalationManager>();
            var escalationManagerResultHandler = new EscalationManagerResultHandler(escalationManagerMock.Object);

            // Act
            escalationManagerResultHandler.Handle(GetDefaultEmployee(), Result.Denied, 0);

            // Assert
            escalationManagerMock.Verify(v => v.NotifyNewPendingRequest(1), Times.Never());
        }

        [TestMethod]
        public void Handle_WhenResultManual_ThenNotifyNewPendingRequestCalled()
        {
            // Arrange
            var escalationManagerMock = new Mock<IEscalationManager>();
            var escalationManagerResultHandler = new EscalationManagerResultHandler(escalationManagerMock.Object);

            // Act
            escalationManagerResultHandler.Handle(GetDefaultEmployee(), Result.Manual, 0);

            // Assert
            escalationManagerMock.Verify(v => v.NotifyNewPendingRequest(1), Times.Once());
        }
    }
}
