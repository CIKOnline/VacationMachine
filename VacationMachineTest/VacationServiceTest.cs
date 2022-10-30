using NSubstitute;
using NUnit.Framework;
using System;
using VacationMachine;

namespace VacationMachineTest
{
    public class VacationServiceTest
    {
        private VacationServiceFake _sut;
        private IVacationDatabase _vacationDatabase;
        private IEmployeeMapper _mapper;

        [SetUp]
        public void Initialize()
        {
            _vacationDatabase = Substitute.For<IVacationDatabase>();
            _mapper = Substitute.For<IEmployeeMapper>();

            _sut = new VacationServiceFake(
                _vacationDatabase,
                _mapper
            );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ValidateRequestedDays_WhenDaysToLow_ThenArgumentException(int days)
        {
            var expectedMessage = $"Invalid amount of days: {days}";

            var exception = Assert.Throws<ArgumentException>(() => _sut.ValidateRequestedDays(days));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        private class VacationServiceFake : VacationService
        {
            public VacationServiceFake(
                IVacationDatabase vacationDatabase,
                IEmployeeMapper mapper
            ) : base(vacationDatabase, mapper)
            {
            }

            public new void ValidateRequestedDays(int days)
            {
                base.ValidateRequestedDays(days);
            }
        }
    }
}
