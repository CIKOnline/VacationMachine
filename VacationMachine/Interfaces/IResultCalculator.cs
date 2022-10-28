using VacationMachine.Enums;

namespace VacationMachine.Interfaces
{
    public interface IResultCalculator
    {
        Result GetResult(int totalDays, EmploymentStatus employeeStatus);
    }
}