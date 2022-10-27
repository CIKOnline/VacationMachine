using VacationMachine.Enums;

namespace VacationMachine
{
    public class ResultCalculator : IResultCalculator
    {
        public Result GetResult(int totalDays, EmploymentStatus employeeStatus)
        {
            if (totalDays > Configuration.GetMaxDays())
            {
                if (employeeStatus.Equals(EmploymentStatus.PERFORMER) && totalDays < Configuration.GetMaxDaysForPerformers())
                {
                    return Result.Manual;
                }
                else
                {

                    return Result.Denied;
                }
            }
            else
            {
                if (employeeStatus.Equals(EmploymentStatus.SLACKER))
                {
                    return Result.Denied;
                }
                else
                {
                    return Result.Approved;
                }
            }
        }
    }
}
