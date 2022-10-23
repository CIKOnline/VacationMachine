using VacationMachine.Database.Schema;

namespace VacationMachine.Database;

public interface IVacationDatabase
{
    Employee FindByEmployeeId(long employeeId);
    void Save(Employee employeeData);
}