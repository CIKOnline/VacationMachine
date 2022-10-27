using VacationMachine.Enums;

namespace VacationMachine
{
    public interface IResultCalculator
    {
        Result GetResult(int totalDays, EmploymentStatus employeeStatus);
    }
}