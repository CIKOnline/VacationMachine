namespace ThirdPartyLibraries.Escalation;

public interface IEscalationManager
{
    void NotifyNewPendingRequest(long employeeId);
}