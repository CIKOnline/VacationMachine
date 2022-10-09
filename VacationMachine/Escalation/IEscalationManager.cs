namespace VacationMachine
{
    public interface IEscalationManager
    {
        void NotifyNewPendingRequest(long employeeId);
    }
}
