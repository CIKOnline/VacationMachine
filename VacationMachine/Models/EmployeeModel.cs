using System.Collections.Generic;
using VacationMachine.Enums;

namespace VacationMachine.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }

        public EmploymentStatus Status { get; set; }

        public List<int> TakenHolidays { get; set; }
    }
}
