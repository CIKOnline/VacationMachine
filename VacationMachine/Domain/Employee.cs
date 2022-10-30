namespace VacationMachine.Domain
{
    public class Employee
    {
        public long EmployeeId { get; set; }
        public int DaysSoFar { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}
