using VacationMachine.Services.Models;

namespace VacationMachine.Services.VacationStrategies
{
    public interface IVacationStrategy
    {
        Result ExecuteStrategy(int days);
    }
}