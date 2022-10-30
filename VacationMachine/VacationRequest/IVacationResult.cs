using VacationMachine.Business;

namespace VacationMachine
{
    public interface IVacationRequest
    {
        public Employee Employee { get; }

        public string StatusMessage { get; }

        public bool IsEmployeeUpdated { get; }

        public void ProcessRequest();
    }
}
