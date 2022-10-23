using System.Collections.Generic;
using VacationMachine.Database.Schema;

namespace VacationMachine.Database;

public class VacationDatabase : IVacationDatabase
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