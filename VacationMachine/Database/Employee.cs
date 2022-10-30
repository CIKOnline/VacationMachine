namespace VacationMachine
{
    public abstract class Employee
    {
        public long EmployeeId { get; set; }
        public int DaysSoFar { get; set; }

        public abstract IRequestResult RequestPaidDaysOff(int days);
    }
}
