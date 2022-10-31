using Moq;
using VacationMachine.Enums;

namespace VacationMachine.Test
{
    [TestClass]
    public class ResultCalculatorTest
    {
        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusSlacker_ThenResultDenied()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 0;
            var employeeStatus = EmploymentStatus.SLACKER;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusRegularAndTotalDaysEqual26_ThenResultApproved()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 26;
            var employeeStatus = EmploymentStatus.REGULAR;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Approved);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusRegularAndTotalDaysEqual27_ThenResultDenied()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 27;
            var employeeStatus = EmploymentStatus.REGULAR;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual26_ThenResultApproved()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 26;
            var employeeStatus = EmploymentStatus.PERFORMER;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Approved);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual27_ThenResultManual()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 27;
            var employeeStatus = EmploymentStatus.PERFORMER;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Manual);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual44_ThenResultManual()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 44;
            var employeeStatus = EmploymentStatus.PERFORMER;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Manual);
        }

        [TestMethod]
        public void RequestPaidDaysOff_WhenStatusPerformerAndTotalDaysEqual45_ThenResultDenied()
        {
            // Arrange
            var resultCalculator = GetResultCalculator();
            var totalDays = 45;
            var employeeStatus = EmploymentStatus.PERFORMER;

            // Act
            var result = resultCalculator.GetResult(totalDays, employeeStatus);

            // Assert
            Assert.AreEqual(result, Result.Denied);
        }

        private ResultCalculator GetResultCalculator()
        {
            return new ResultCalculator();
        }
    }
}