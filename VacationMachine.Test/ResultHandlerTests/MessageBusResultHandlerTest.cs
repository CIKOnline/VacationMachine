using Moq;
using VacationMachine.Enums;
using VacationMachine.ResultHandler;

namespace VacationMachine.Test.ResultHandlerTests
{
    [TestClass]
    public class MessageBusResultHandlerTest : ResultHandlerBaseTest
    {
        [TestMethod]
        public void Handle_WhenResultApproved_ThenSendEventCalled()
        {
            // Arrange
            var messageBusMock = new Mock<IMessageBus>();
            var messageBusResultHandler = new MessageBusResultHandler(messageBusMock.Object);

            // Act
            messageBusResultHandler.Handle(GetDefaultEmployee(), Result.Approved, 0);

            // Assert
            messageBusMock.Verify(v => v.SendEvent(Configuration.GetEventText()), Times.Once());
        }

        [TestMethod]
        public void Handle_WhenResultDenied_ThenSendEventNotCalled()
        {
            // Arrange
            var messageBusMock = new Mock<IMessageBus>();
            var messageBusResultHandler = new MessageBusResultHandler(messageBusMock.Object);

            // Act
            messageBusResultHandler.Handle(GetDefaultEmployee(), Result.Denied, 0);

            // Assert
            messageBusMock.Verify(v => v.SendEvent(Configuration.GetEventText()), Times.Never());
        }

        [TestMethod]
        public void Handle_WhenResultManual_ThenSendEventNotCalled()
        {
            // Arrange
            var messageBusMock = new Mock<IMessageBus>();
            var messageBusResultHandler = new MessageBusResultHandler(messageBusMock.Object);

            // Act
            messageBusResultHandler.Handle(GetDefaultEmployee(), Result.Manual, 0);

            // Assert
            messageBusMock.Verify(v => v.SendEvent(Configuration.GetEventText()), Times.Never());
        }
    }
}
