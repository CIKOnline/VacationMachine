using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public abstract class Employee
    {
        public abstract EmployeeRole Role { get; }
        public long EmployeeId { get; set; }
        public int DaysSoFar { get; set; }

        public abstract IVacationRequest RequestPaidDaysOff(int days);
    }
}
