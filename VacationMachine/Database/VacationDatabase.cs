using VacationMachine.Enums;
using VacationMachine.Interfaces;
using VacationMachine.Models;

namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        public void AddEmployeeHolidays(long employeeId, int days)
        {
            var employee = FindByEmployeeId(employeeId);

            employee.TakenHolidays += days;

            Save(employee);
        }

        public EmployeeModel FindByEmployeeId(long employeeId)
        {
            return new EmployeeModel
            {
                Id = employeeId,
                Status = EmploymentStatus.SLACKER,
                TakenHolidays = 1
            };
        }

        public void Save(EmployeeModel employeeData)
        {

        }
    }
}
