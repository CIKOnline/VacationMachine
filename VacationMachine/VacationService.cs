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
                        ApprovePaidDaysOff(days, employee);
                        return Result.Approved;
                    }
                    else if (newDaysSoFar < 45)
                    {
                        ManualPaidDaysOff(employee);
                        return Result.Manual;
                    }
                    break;

                case EmployeeStatus.Regular:
                    if (employee.DaysSoFar + days <= 26)
                    {
                        ApprovePaidDaysOff(days, employee);
                        return Result.Approved;
                    }
                    break;

                case EmployeeStatus.Slacker:
                    break;

                default:
                    throw new NotImplementedException(nameof(employee.Status));
            }

            DeniePaidDaysOff();
            return Result.Denied;
        }

        private void ManualPaidDaysOff(Employee employee)
        {
            _escalationManager.NotifyNewPendingRequest(employee.EmployeeId);
        }

        private void DeniePaidDaysOff()
        {
            _emailSender.Send("next time");
        }

        private void ApprovePaidDaysOff(int days, Employee employee)
        {
            employee.DaysSoFar += days;
            _vacationDatabase.Save(employee);
            _messageBus.SendEvent("request approved");
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
