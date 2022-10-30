using System;
using VacationMachine.Business;
using VacationMachine.Domain;

namespace VacationMachine
{
    public class EmployeeMapper : IEmployeeMapper
    {
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public EmployeeMapper(
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        )
        {
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public Domain.Employee ToDomain(Business.Employee businessEmployee)
        {
            return new Domain.Employee
            {
                EmployeeId = businessEmployee.EmployeeId,
                DaysSoFar = businessEmployee.DaysSoFar,
                Status = businessEmployee.Role
            };
        }

        public Business.Employee ToBusiness(Domain.Employee domainEmployee)
        {
            Business.Employee businessEmployee = domainEmployee.Status switch
            {
                EmployeeRole.Performer => new PerformerEmployee(
                    _messageBus,
                    _emailSender,
                    _escalationManager
                ),
                EmployeeRole.Regular => new RegularEmployee(
                    _messageBus,
                    _emailSender
                ),
                EmployeeRole.Slacker => new SlackerEmployee(
                    _emailSender
                ),
                _ => throw new NotImplementedException(),
            };

            businessEmployee.EmployeeId = domainEmployee.EmployeeId;
            businessEmployee.DaysSoFar = domainEmployee.DaysSoFar;

            return businessEmployee;
        }
    }
}
