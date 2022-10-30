using VacationMachine.Domain;

namespace VacationMachine.Business
{
    public class RegularEmployee : Employee
    {
        public override EmployeeStatus Status => EmployeeStatus.Regular;

        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IEmailSender _emailSender;

        public RegularEmployee(
            IVacationDatabase vacationDatabase,
            IMapper mapper,
            IMessageBus messageBus,
            IEmailSender emailSender
        )
        {
            _vacationDatabase = vacationDatabase;
            _mapper = mapper;
            _messageBus = messageBus;
            _emailSender = emailSender;
        }

        public override IRequestResult RequestPaidDaysOff(int days)
        {
            if (DaysSoFar + days <= Configuration.MAX_DAYS)
            {
                return new ApprovedRequestResult(_vacationDatabase, _mapper, _messageBus, this, days);
            }
            return new DeniedRequestResult(_emailSender);
        }
    }
}
