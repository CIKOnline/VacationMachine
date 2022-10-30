using NSubstitute;
using NUnit.Framework;
using System;
using VacationMachine;

namespace VacationMachineTest
{
    public class VacationServiceTest
    {
        private VacationServiceFake _sut;

        [SetUp]
        public void Initialize()
        {
            var employeeManager = Substitute.For<IEmployeeManager>();

            _sut = new VacationServiceFake(employeeManager);
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateRequestedDays_WhenDaysInvalid_ThenArgumentException(int days)
        {
            var expectedMessage = $"Invalid amount of days: {days}";

            var exception = Assert.Throws<ArgumentException>(() => _sut.ValidateRequestedDays(days));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [Test]
        [TestCase(1)]
        public void ValidateRequestedDays_WhenDaysValid_ThenNoException(int days)
        {
            _sut.ValidateRequestedDays(days);
        }

        private class VacationServiceFake : VacationService
        {
            public VacationServiceFake(IEmployeeManager employeeManager) : base(employeeManager)
            {
            }

            public new void ValidateRequestedDays(int days)
            {
                base.ValidateRequestedDays(days);
            }
        }
    }
}
