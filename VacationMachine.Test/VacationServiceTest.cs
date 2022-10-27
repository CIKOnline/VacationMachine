using Moq;

namespace VacationMachine.Test
{
    [TestClass]
    public class VacationServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestPaidDaysOff_WhenDaysMinusOne_ThenThrow()
        {
            // Arrange
            var databaseMock = GetDatabase(null);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            vacationService.RequestPaidDaysOff(-1, 1);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusSlacker_ThenResultDenied()
        {
            // Arrange
            var employee = new object[] { "SLACKER", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(0, 1);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusRegularAndTotalDaysEqual26_ThenResultApproved()
        {
            // Arrange
            var employee = new object[] { "REGULAR", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(26, 1);

            // Assert
            Assert.AreEqual(result, Result.Approved);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusRegularAndTotalDaysEqual27_ThenResultDenied()
        {
            // Arrange
            var employee = new object[] { "REGULAR", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(27, 1);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual26_ThenResultApproved()
        {
            // Arrange
            var employee = new object[] { "PERFORMER", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(26, 1);

            // Assert
            Assert.AreEqual(result, Result.Approved);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual27_ThenResultManual()
        {
            // Arrange
            var employee = new object[] { "PERFORMER", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(27, 1);

            // Assert
            Assert.AreEqual(result, Result.Manual);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual44_ThenResultManual()
        {
            // Arrange
            var employee = new object[] { "PERFORMER", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(44, 1);

            // Assert
            Assert.AreEqual(result, Result.Manual);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual45_ThenResultDenied()
        {
            // Arrange
            var employee = new object[] { "PERFORMER", 0 };
            var databaseMock = GetDatabase(employee);
            var messageBusMock = GetMessageBus();
            var emailSenderMock = GetEmailSender();
            var escalationManagerMock = GetEscalationManager();
            var vacationService = GetVacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);

            // Act
            var result = vacationService.RequestPaidDaysOff(45, 1);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        private VacationService GetVacationService(IVacationDatabase databaseMock, IMessageBus messageBusMock, IEmailSender emailSenderMock, IEscalationManager escalationManagerMock)
        {
            return new VacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock);
        }

        private IVacationDatabase GetDatabase(object[] employee)
        {
            var databaseMock = new Mock<IVacationDatabase>();

            databaseMock.Setup(db => db.FindByEmployeeId(It.IsAny<long>())).Returns(employee);

            return databaseMock.Object;
        }

        private IMessageBus GetMessageBus()
        {
            var messageBusMock = new Mock<IMessageBus>();

            //messageBusMock.Setup(mb => mb.SendEvent(It.IsAny<string>()));

            return messageBusMock.Object;
        }

        private IEmailSender GetEmailSender()
        {
            var emailSenderMock = new Mock<IEmailSender>();

            //emailSenderMock.Setup(es => es.Send(It.IsAny<string>()));

            return emailSenderMock.Object;
        }

        private IEscalationManager GetEscalationManager()
        {
            var escalationManagerMock = new Mock<IEscalationManager>();

            //emailSenderMock.Setup(es => es.Send(It.IsAny<string>()));

            return escalationManagerMock.Object;
        }
    }
}