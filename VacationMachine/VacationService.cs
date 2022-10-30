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
            var employee = _vacationDatabase.FindByEmployeeId(employeeId);

            return RequestPaidDaysOff(days, employee);
        }

        protected Result RequestPaidDaysOff(int days, Employee employee)
        {
            ValidateRequestedDays(days);

            switch (employee.Status)
            {
                case EmployeeStatus.Performer:
                    var newDaysSoFar = employee.DaysSoFar + days;
                    if (newDaysSoFar <= 26)
                    {
                        employee.DaysSoFar += days;
                        _vacationDatabase.Save(employee);
                        _messageBus.SendEvent("request approved");
                        return Result.Approved;
                    }
                    else if (newDaysSoFar < 45)
                    {
                        _escalationManager.NotifyNewPendingRequest(employee.EmployeeId);
                        return Result.Manual;
                    }
                    _emailSender.Send("next time");
                    return Result.Denied;

                case EmployeeStatus.Regular:
                    if (employee.DaysSoFar + days > 26)
                    {
                        _emailSender.Send("next time");
                        return Result.Denied;
                    }
                    employee.DaysSoFar += days;
                    _vacationDatabase.Save(employee);
                    _messageBus.SendEvent("request approved");
                    return Result.Approved;

                case EmployeeStatus.Slacker:
                    _emailSender.Send("next time");
                    return Result.Denied;

                default:
                    throw new NotImplementedException(nameof(employee.Status));
            }
        }

        protected void ValidateRequestedDays(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException($"Invalid amount of days: {days}");
            }
        }
    }
}
