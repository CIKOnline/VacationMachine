using System;

namespace VacationMachine
{
    public class VacationService
    {
        private readonly IVacationDatabase _database;
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
            _database = database;
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
            var employeeData = _database.FindByEmployeeId(employeeId);
            var employeeStatus = (string)employeeData[0];
            var daysSoFar = (int)employeeData[1];

            if (daysSoFar + days > 26)
            {
                if (employeeStatus.Equals("PERFORMER") && daysSoFar + days < 45)
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
                if (employeeStatus.Equals("SLACKER"))
                {
                    result = Result.Denied;
                    _emailSender.Send("next time");
                }
                else
                {
                    employeeData[1] = daysSoFar + days;
                    result = Result.Approved;
                    _database.Save(employeeData);
                    _messageBus.SendEvent("request approved");
                }
            }

            return result;
        }
    }
}
