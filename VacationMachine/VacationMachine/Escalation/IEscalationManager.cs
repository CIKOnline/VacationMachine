namespace VacationMachine.Escalation;

public interface IEscalationManager
{
    void NotifyNewPendingRequest(long employeeId);
}