using System;
using VacationMachine.Business;
using VacationMachine.Domain;

namespace VacationMachine
{
    public class Mapper : IMapper
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public Mapper(
            IVacationDatabase vacationDatabase,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager)
        {
            _vacationDatabase = vacationDatabase;
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
                Status = businessEmployee.Status
            };
        }

        public Business.Employee ToBusiness(Domain.Employee domainEmployee)
        {
            Business.Employee businessEmployee;

            switch (domainEmployee.Status)
            {
                case EmployeeStatus.Performer:
                    businessEmployee = new PerformerEmployee(
                        _messageBus,
                        _emailSender,
                        _escalationManager
                    );
                    break;

                case EmployeeStatus.Regular:
                    businessEmployee = new RegularEmployee(
                        _vacationDatabase,
                        this,
                        _messageBus,
                        _emailSender
                    );
                    break;

                case EmployeeStatus.Slacker:
                    businessEmployee = new SlackerEmployee(
                        _emailSender
                    );
                    break;

                default:
                    throw new NotImplementedException();
            }

            businessEmployee.EmployeeId = domainEmployee.EmployeeId;
            businessEmployee.DaysSoFar = domainEmployee.DaysSoFar;

            return businessEmployee;
        }
    }
}
