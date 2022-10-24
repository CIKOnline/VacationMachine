using ThirdPartyLibraries.Escalation;
using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.DataAccess.DataModels;

namespace VacationMachine.BusinessLogic.ResultHandlers.Concrete;

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