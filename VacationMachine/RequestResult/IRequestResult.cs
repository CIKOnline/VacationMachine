using VacationMachine.Business;

namespace VacationMachine
{
    public interface IRequestResult
    {
        public Employee Employee { get; }

        public string StatusMessage { get; }

        public bool IsEmployeeChanged { get; }

        public void ProcessRequest();
    }
}
