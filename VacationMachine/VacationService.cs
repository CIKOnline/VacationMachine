using System;
using VacationMachine.Enums;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _database;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;
        private readonly IResultCalculator _resultCalculator;

        public VacationService(
            IVacationDatabase database, 
            IMessageBus messageBus, 
            IEmailSender emailSender,
            IEscalationManager escalationManager,
            IResultCalculator resultCalculator)
        {
            _database = database;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
            _resultCalculator = resultCalculator;
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

            if (result.Equals(Result.Denied))
            {
                _emailSender.Send("next time");
            }

            if (result.Equals(Result.Approved))
            {
                _database.AddEmployeeHolidays(employeeId, days);
                _messageBus.SendEvent("request approved");
            }

            if (result.Equals(Result.Manual))
            {
                _escalationManager.NotifyNewPendingRequest(employeeId);
            }

            return result;
        }
    }
}
