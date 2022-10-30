using VacationMachine.Business;

namespace VacationMachine
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IEmployeeMapper _mapper;

        public EmployeeManager(
            IVacationDatabase vacationDatabase,
            IEmployeeMapper mapper
        )
        {
            _vacationDatabase = vacationDatabase;
            _mapper = mapper;
        }

        public Employee FindByEmployeeId(long employeeId)
        {
            var domainEmployee = _vacationDatabase.FindByEmployeeId(employeeId);
            var employee = _mapper.ToBusiness(domainEmployee);
            return employee;
        }

        public void SaveEmployee(Employee employee)
        {
            var domainEmployee = _mapper.ToDomain(employee);
            _vacationDatabase.Save(domainEmployee);
        }
    }
}
