using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public abstract class Employee
    {
        public abstract EmployeeStatus Status { get; }
        public long EmployeeId { get; set; }
        public int DaysSoFar { get; set; }

        public abstract IRequestResult RequestPaidDaysOff(int days);
    }
}
