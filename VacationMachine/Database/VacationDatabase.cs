using VacationMachine.Domain;

namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        public Employee FindByEmployeeId(long employeeId)
        {
            return new Employee()
            {
                EmployeeId = employeeId,
                DaysSoFar = 1,
                Status = EmployeeStatus.Slacker
            };
        }

        public void Save(Employee employeeData)
        {
        }
    }
}
