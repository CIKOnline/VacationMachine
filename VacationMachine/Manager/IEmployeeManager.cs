using VacationMachine.Business;

namespace VacationMachine
{
    public interface IEmployeeManager
    {
        public Employee FindByEmployeeId(long employeeId);

        public void SaveEmployee(Employee employee);
    }
}
