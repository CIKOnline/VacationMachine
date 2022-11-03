using VacationMachine.Services.Models;

namespace VacationMachine.Services
{
    public interface IVacationService
    {
        Result RequestPaidDaysOff(int days, long employeeId);
    }
}