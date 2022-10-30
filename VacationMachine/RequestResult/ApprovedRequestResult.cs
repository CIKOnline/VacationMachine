namespace VacationMachine
{
    public class ApprovedRequestResult : IRequestResult
    {
        private readonly IVacationDatabase _vacationDatabase;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly Business.Employee _employee;
        private readonly int days;

        public ApprovedRequestResult(
            IVacationDatabase vacationDatabase,
            IMapper mapper,
            IMessageBus messageBus,
            Business.Employee employee,
            int days
        )
        {
            _vacationDatabase = vacationDatabase;
            _mapper = mapper;
            _messageBus = messageBus;
            _employee = employee;
            this.days = days;
        }

        public string Name => "Approved";

        public void ProcessRequest()
        {
            _employee.DaysSoFar += days;
            var employee = _mapper.ToDomain(_employee);
            _vacationDatabase.Save(employee);
            _messageBus.SendEvent("request approved");
        }
    }
}
