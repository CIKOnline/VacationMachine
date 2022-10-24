using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VacationMachine.AppSettings;
using VacationMachine.BusinessLogic.ResultHandlers.Interfaces;
using VacationMachine.BusinessLogic.Services.Interfaces;
using VacationMachine.DataAccess.DataModels;
using VacationMachine.DataAccess.DataModels.Enums;
using VacationMachine.DataAccess.Repositories.Interfaces;
using VacationMachine.Models;

namespace VacationMachine.BusinessLogic.Services.Concrete;

public class VacationRequestProcessorService : IVacationRequestProcessorService
{
    private readonly IVacationRepository _repository;
    private readonly List<IResultHandler> _resultHandlers;
    private readonly IVacationCalculator _vacationCalculator;
    private readonly VacationDaysLimitSettings _vacationDaysLimitSettings;

    public VacationRequestProcessorService(
        IVacationRepository repository,
        IVacationCalculator vacationCalculator,
        IEnumerable<IResultHandler> resultHandlers,
        IOptions<VacationDaysLimitSettings> options)
    {
        _repository = repository;
        _vacationCalculator = vacationCalculator;
        _resultHandlers = resultHandlers.ToList();
        _vacationDaysLimitSettings = options.Current;
    }

    public Result RequestPaidDaysOff(RequestModel requestModel)
    {
        if (requestModel.DaysToTake <= 0)
            throw new ArgumentException();

        Employee employeeData = _repository.FindByEmployeeId(requestModel.EmployeeId);

        RequestCalculationModel requestCalculationModel = new()
        {
            DaysLimit = GetDefaultLimitForSpecificEmployeeStatus(employeeData.Status),
            AdditionalDaysLimit = GetAdditionalLimitForSpecificEmployeeStatus(employeeData.Status),
            DaysToTake = requestModel.DaysToTake,
            DaysTakenSoFar = employeeData.ApprovedVacationRequestsList.Sum(a => a.Days)
        };
        Result result = _vacationCalculator.CalculateRequest(requestCalculationModel);
        HandleResult(result, employeeData, requestModel.DaysToTake);

        return result;
    }

    private void HandleResult(Result result, Employee employee, long daysToTake)
    {
        foreach (IResultHandler resultHandler in _resultHandlers)
            resultHandler.Handle(result, employee, daysToTake);
    }

    private long GetDefaultLimitForSpecificEmployeeStatus(EmployeeStatus employeeStatus)
    {
        return GetValueFromPropertyOrDefault(
            _vacationDaysLimitSettings.Default,
            employeeStatus.ToString(),
            _vacationDaysLimitSettings.Default.Default);
    }

    private long GetAdditionalLimitForSpecificEmployeeStatus(EmployeeStatus employeeStatus)
    {
        return GetValueFromPropertyOrDefault(
            _vacationDaysLimitSettings.Additional,
            employeeStatus.ToString(),
            _vacationDaysLimitSettings.Additional.Default);
    }

    private T GetValueFromPropertyOrDefault<T>(object @object, string propertyName, T defaultValue)
    {
        Type settingsType = @object.GetType();
        PropertyInfo specificSettingsProperty = settingsType.GetProperty(propertyName);
        object specificSettingsValue = specificSettingsProperty?.GetValue(@object);
        if (specificSettingsValue is null)
            return defaultValue;

        return (T)specificSettingsValue;
    }
}