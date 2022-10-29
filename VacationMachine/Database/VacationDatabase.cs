using System.Collections.Generic;
using VacationMachine.Enums;
using VacationMachine.Interfaces;
using VacationMachine.Models;

namespace VacationMachine
{
    public class VacationDatabase : IVacationDatabase
    {
        public EmployeeModel FindByEmployeeId(long employeeId)
        {
            return new EmployeeModel
            {
                Id = employeeId,
                Status = EmploymentStatus.SLACKER,
                TakenHolidays = new List<int>() { 1 }
            };
        }

        public void Save(EmployeeModel employeeData)
        {

        }
    }
}
