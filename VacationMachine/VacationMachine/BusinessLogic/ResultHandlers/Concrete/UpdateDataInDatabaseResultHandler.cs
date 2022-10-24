using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.DataAccess.DataModels;
using VacationMachine.DataAccess.Repositories.Interfaces;

namespace VacationMachine.BusinessLogic.ResultHandlers.Concrete;

public class UpdateDataInDatabaseResultHandler : IResultHandler
{
    private readonly IVacationRepository _vacationRepository;

    public UpdateDataInDatabaseResultHandler(IVacationRepository vacationRepository)
    {
        _vacationRepository = vacationRepository;
    }

    public void Handle(Result result, Employee employee, long daysToTake)
    {
        if (result != Result.Approved)
            return;

        employee.ApprovedVacationRequestsList.Add(new ApprovedVacationRequests
        {
            Days = daysToTake
        });
        _vacationRepository.Save(employee);
    }
}