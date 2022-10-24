namespace VacationMachine.Models;

public class RequestCalculationModel
{
    public long DaysLimit { get; init; }
    public long AdditionalDaysLimit { get; init; }
    public long DaysToTake { get; init; }
    public long DaysTakenSoFar { get; init; }

    public long DaysTakenIfApproved => DaysToTake + DaysTakenSoFar;
    public long ExtendedDaysLimit => DaysLimit + AdditionalDaysLimit;
}