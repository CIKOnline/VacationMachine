using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public class PerformerEmployee : Employee
    {
        public override EmployeeStatus Status => EmployeeStatus.Performer;

        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;
        private readonly IEscalationManager _escalationManager;

        public PerformerEmployee(
            IVacationDatabase vacationDatabase,
            IMapper mapper,
            IMessageBus messageBus,
            IEmailSender emailSender,
            IEscalationManager escalationManager
        )
        {
            _vacationDatabase = vacationDatabase;
            _mapper = mapper;
            _messageBus = messageBus;
            _emailSender = emailSender;
            _escalationManager = escalationManager;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            var newDaysSoFar = DaysSoFar + days;
            if (newDaysSoFar <= Configuration.MAX_DAYS)
            {
                return new ApprovedRequestResult(_vacationDatabase, _mapper, _messageBus, this, days);
            }
            else if (newDaysSoFar <= Configuration.MAX_DAYS_FOR_PERFORMERS)
            {
                return new ManualRequestResult(_escalationManager, EmployeeId);
            }
            return new DeniedRequestResult(_emailSender);
        }
    }
}
