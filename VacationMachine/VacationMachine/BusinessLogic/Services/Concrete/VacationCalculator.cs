using VacationMachine.BusinessLogic.Services.Interfaces;
using VacationMachine.Models;

namespace VacationMachine.BusinessLogic.Services.Concrete;

public class VacationCalculator : IVacationCalculator
{
    public Result CalculateRequest(RequestCalculationModel model)
    {
        if (model.DaysLimit >= model.DaysTakenIfApproved)
            return Result.Approved;
        if (model.ExtendedDaysLimit >= model.DaysTakenIfApproved)
            return Result.Manual;

        return Result.Denied;
    }
}