using VacationMachine.Enums;
using VacationMachine.Models;

namespace VacationMachine.Test.ResultHandlerTests
{
    public class ResultHandlerBaseTest
    {
        protected EmployeeModel GetDefaultEmployee()
        {
            return new EmployeeModel
            {
                Id = 1,
                Status = EmploymentStatus.SLACKER,
                TakenHolidays = new List<int> { 1 }
            };
        }
    }
}