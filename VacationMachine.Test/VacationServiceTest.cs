using Moq;
using VacationMachine.Enums;

namespace VacationMachine.Test
{
    [TestClass]
    public class VacationServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestPaidDaysOff_WhenDaysBelowZero_ThenThrow()
        {
            // Arrange
            var vacationService = GetVacationService(
                GetDatabaseMock().Object, 
                GetMessageBusMock().Object, 
                GetEmailSenderMock().Object, 
                GetEscalationManagerMock().Object, 
                GetResultCalculator(Result.Denied)
            );

            // Act
            vacationService.RequestPaidDaysOff(-1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestPaidDaysOff_WhenResultApproved_ThenAddEmployeeHolidaysAndSendEventIsCalled()
        {
            // Arrange
            var databaseMock = GetDatabaseMock();
            var messageBusMock = GetMessageBusMock();
            var result = Result.Approved;
            var vacationService = GetVacationService(databaseMock.Object, messageBusMock.Object, GetEmailSenderMock().Object, GetEscalationManagerMock().Object, GetResultCalculator(result));

            // Act
            vacationService.RequestPaidDaysOff(-1, 1);

            // Assert
            databaseMock.Verify(v => v.AddEmployeeHolidays(It.IsAny<long>(), It.IsAny<int>()));
            messageBusMock.Verify(v => v.SendEvent(It.IsAny<string>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestPaidDaysOff_WhenResultDenied_ThenEmailSent()
        {
            // Arrange
            var emailSenderMock = GetEmailSenderMock();
            var result = Result.Denied;
            var vacationService = GetVacationService(GetDatabaseMock().Object, GetMessageBusMock().Object, GetEmailSenderMock().Object, GetEscalationManagerMock().Object, GetResultCalculator(result));

            // Act
            vacationService.RequestPaidDaysOff(-1, 1);

            // Assert
            emailSenderMock.Verify(v => v.Send(It.IsAny<string>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RequestPaidDaysOff_WhenResultManual_ThenNotifyNewPendingRequest()
        {
            // Arrange
            var emailSenderMock = GetEscalationManagerMock();
            var result = Result.Denied;
            var vacationService = GetVacationService(GetDatabaseMock().Object, GetMessageBusMock().Object, GetEmailSenderMock().Object, GetEscalationManagerMock().Object, GetResultCalculator(result));

            // Act
            vacationService.RequestPaidDaysOff(-1, 1);

            // Assert
            emailSenderMock.Verify(v => v.NotifyNewPendingRequest(It.IsAny<long>()));
        }

        private VacationService GetVacationService(IVacationDatabase databaseMock, IMessageBus messageBusMock, IEmailSender emailSenderMock, IEscalationManager escalationManagerMock, IResultCalculator resultCalculator)
        {
            return new VacationService(databaseMock, messageBusMock, emailSenderMock, escalationManagerMock, resultCalculator);
        }

        private Mock<IVacationDatabase> GetDatabaseMock()
        {
            return new Mock<IVacationDatabase>();
        }

        private Mock<IMessageBus> GetMessageBusMock()
        {
            return new Mock<IMessageBus>();
        }

        private Mock<IEmailSender> GetEmailSenderMock()
        {
            return new Mock<IEmailSender>();
        }

        private Mock<IEscalationManager> GetEscalationManagerMock()
        {
            return new Mock<IEscalationManager>();
        }

        private IResultCalculator GetResultCalculator(Result result)
        {
            var resultCalculatorMock = new Mock<IResultCalculator>();

            resultCalculatorMock.Setup(mb => mb.GetResult(It.IsAny<int>(), It.IsAny<EmploymentStatus>())).Returns(result);

            return resultCalculatorMock.Object;
        }
    }
}