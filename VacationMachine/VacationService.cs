using System;
using System.Collections.Generic;
using System.Linq;
using VacationMachine.Enums;
using VacationMachine.Interfaces;
using VacationMachine.ResultHandler.Interfaces;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _database;
        private readonly IResultCalculator _resultCalculator;
        private readonly IEnumerable<IResultHandler> _resultHandlers;

        public VacationService(
            IVacationDatabase database,
            IResultCalculator resultCalculator,
            IEnumerable<IResultHandler> resultHandlers)
        {
            _database = database;
            _resultCalculator = resultCalculator;
            _resultHandlers = resultHandlers;
        }

        public Result RequestPaidDaysOff(int days, long employeeId)
        {
            if (days < 0)
            {
                throw new ArgumentException();
            }

            var employee = _database.FindByEmployeeId(employeeId);
            var totalDays = employee.TakenHolidays + days;
            var result = _resultCalculator.GetResult(totalDays, employee.Status);

            CallServicesByResult(result, employeeId, days);

            return result;
        }

        private void CallServicesByResult(Result result, long employeeId, int days)
        {
            if (result.Equals(Result.Approved))
            {
                _database.AddEmployeeHolidays(employeeId, days);
            }

            _resultHandlers.ToList().ForEach(h => h.Handle(employeeId, result, days));            
        }
    }
}
