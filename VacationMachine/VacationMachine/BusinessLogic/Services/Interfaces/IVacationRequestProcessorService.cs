using VacationMachine.Models;

namespace VacationMachine.BusinessLogic.Services.Interfaces;

public interface IVacationRequestProcessorService
{
    Result RequestPaidDaysOff(RequestModel requestModel);
}