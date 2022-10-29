using VacationMachine.Models;

namespace VacationMachine.Interfaces
{
    public interface IVacationDatabase
    {
        EmployeeModel FindByEmployeeId(long employeeId);
        void Save(EmployeeModel employeeData);
    }
}
