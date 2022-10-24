using VacationMachine.Models;

namespace VacationMachine.BusinessLogic.Services.Interfaces;

public interface IVacationCalculator
{
    Result CalculateRequest(RequestCalculationModel requestCalculationModel);
}