using VacationMachine.Database;
using VacationMachine.Database.Schema;

namespace VacationMachine.ResultHandlers;

public class UpdateDataInDatabaseResultHandler : IResultHandler
{
    private readonly IVacationDatabase _vacationDatabase;

    public UpdateDataInDatabaseResultHandler(IVacationDatabase vacationDatabase)
    {
        _vacationDatabase = vacationDatabase;
    }
    public void Handle(Result result, Employee employee, long daysToTake)
    {
        if (result != Result.Approved)
            return;

        employee.ApprovedVacationRequestsList.Add(new ApprovedVacationRequests
        {
            Days = daysToTake
        });
        _vacationDatabase.Save(employee);
    }
}