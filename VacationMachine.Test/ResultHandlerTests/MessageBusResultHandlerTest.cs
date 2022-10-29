using Moq;
using VacationMachine.Enums;
using VacationMachine.ResultHandler;

namespace VacationMachine.Test.ResultHandlerTests
{
    [TestClass]
    public class MessageBusResultHandlerTest
    {
        [TestMethod]
        public void Handle_WhenResultApproved_ThenSendEventCalled()
        {
            // Arrange
            var messageBusMock = new Mock<IMessageBus>();
            var messageBusResultHandler = new MessageBusResultHandler(messageBusMock.Object);

            // Act
            messageBusResultHandler.Handle(1, Result.Approved, 0);

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
            messageBusResultHandler.Handle(1, Result.Denied, 0);

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
            messageBusResultHandler.Handle(1, Result.Manual, 0);

            // Assert
            messageBusMock.Verify(v => v.SendEvent(Configuration.GetEventText()), Times.Never());
        }
    }
}
