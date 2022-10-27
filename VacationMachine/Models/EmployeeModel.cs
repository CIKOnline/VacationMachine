using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationMachine.Enums;

namespace VacationMachine.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }

        public EmploymentStatus Status { get; set; }

        public int TakenHolidays { get; set; }
    }
}
