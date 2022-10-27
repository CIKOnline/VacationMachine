using VacationMachine.Models;

namespace VacationMachine
{
    public interface IVacationDatabase
    {
        void AddEmployeeHolidays(long employeeId, int days);
        EmployeeModel FindByEmployeeId(long employeeId);
        void Save(EmployeeModel employeeData);
    }
}
