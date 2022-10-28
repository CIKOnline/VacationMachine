using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationMachine.Enums;
using VacationMachine.Models;

namespace VacationMachine.ResultHandler.Interfaces
{
    public interface IResultHandler
    {
        void Handle(long employeeId, Result result, int daysToTake);
    }
}
