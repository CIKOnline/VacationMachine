using VacationMachine.DataAccess.DataModels;

namespace VacationMachine.DataAccess.Repositories.Interfaces;

public interface IVacationRepository
{
    Employee FindByEmployeeId(long employeeId);
    void Save(Employee employeeData);
}