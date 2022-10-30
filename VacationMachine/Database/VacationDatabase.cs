namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        public Employee FindByEmployeeId(long employeeId)
        {
            return new Employee
            {
                Status = EmployeeStatus.Slacker,
                DaysSoFar = 1
            };
        }

        public void Save(Employee employeeData)
        {
        }
    }
}
