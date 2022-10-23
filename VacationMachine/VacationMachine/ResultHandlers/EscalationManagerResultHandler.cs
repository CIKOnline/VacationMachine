using VacationMachine.Database.Schema;
using VacationMachine.Escalation;

namespace VacationMachine.ResultHandlers;

public class EscalationManagerResultHandler : IResultHandler
{
    private readonly IEscalationManager _escalationManager;

    public EscalationManagerResultHandler(IEscalationManager escalationManager)
    {
        _escalationManager = escalationManager;
    }

    public void Handle(Result result, Employee employee, long daysToTake)
    {
        if (result == Result.Manual)
            _escalationManager.NotifyNewPendingRequest(employee.EmployeeId);
    }
}