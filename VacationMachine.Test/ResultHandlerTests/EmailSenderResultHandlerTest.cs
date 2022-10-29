﻿using Moq;
using VacationMachine.Enums;
using VacationMachine.ResultHandler;

namespace VacationMachine.Test.ResultHandlerTests
{
    [TestClass]
    public class EmailSenderResultHandlerTest
    {
        [TestMethod]
        public void Handle_WhenResultApproved_ThenSendNotCalled()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var emailSenderResultHandler = new EmailSenderResultHandler(emailSenderMock.Object);

            // Act
            emailSenderResultHandler.Handle(0, Result.Approved, 0);

            // Assert
            emailSenderMock.Verify(v => v.Send(Configuration.GetEmailText()), Times.Never());
        }

        [TestMethod]
        public void Handle_WhenResultDenied_ThenSendCalled()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var emailSenderResultHandler = new EmailSenderResultHandler(emailSenderMock.Object);

            // Act
            emailSenderResultHandler.Handle(0, Result.Denied, 0);

            // Assert
            emailSenderMock.Verify(v => v.Send(Configuration.GetEmailText()));
        }

        [TestMethod]
        public void Handle_WhenResultManual_ThenSendNotCalled()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var emailSenderResultHandler = new EmailSenderResultHandler(emailSenderMock.Object);

            // Act
            emailSenderResultHandler.Handle(0, Result.Manual, 0);

            // Assert
            emailSenderMock.Verify(v => v.Send(Configuration.GetEmailText()), Times.Never());
        }
    }
}