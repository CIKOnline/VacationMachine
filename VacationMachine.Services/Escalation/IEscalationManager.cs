namespace VacationMachine.Services.Escalation
{
    public interface IEscalationManager
    {
        void NotifyNewPendingRequest(long employeeId);
    }
}
