using System;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public VacationService(
            IVacationDatabase database,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        )
        {
            _vacationDatabase = database;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public Result RequestPaidDaysOff(int days, long employeeId)
        {
            if (days <= 0)
            {
                throw new ArgumentException($"Invalid amount of days: {days}");
            }

            Result result;
            var employee = _vacationDatabase.FindByEmployeeId(employeeId);

            if (employee.DaysSoFar + days > 26)
            {
                if (employee.Status.Equals(EmployeeStatus.Performer) && employee.DaysSoFar + days < 45)
                {
                    result = Result.Manual;
                    _escalationManager.NotifyNewPendingRequest(employeeId);
                }
                else
                {
                    result = Result.Denied;
                    _emailSender.Send("next time");
                }
            }
            else
            {
                if (employee.Status.Equals(EmployeeStatus.Slacker))
                {
                    result = Result.Denied;
                    _emailSender.Send("next time");
                }
                else
                {
                    employee.DaysSoFar += days;
                    result = Result.Approved;
                    _vacationDatabase.Save(employee);
                    _messageBus.SendEvent("request approved");
                }
            }

            return result;
        }
    }
}
