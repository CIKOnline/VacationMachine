using System.Collections.Generic;
using VacationMachine.DataAccess.DataModels;
using VacationMachine.DataAccess.Repositories.Interfaces;

namespace VacationMachine.DataAccess.Repositories.Concrete;

public class VacationRepository : IVacationRepository
{
    private readonly Dictionary<long, Employee> _employees = new();

    public Employee FindByEmployeeId(long employeeId)
    {
        return _employees[employeeId];
    }

    public void Save(Employee employeeData)
    {
        _employees[employeeData.EmployeeId] = employeeData;
    }
}