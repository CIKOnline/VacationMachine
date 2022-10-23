using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VacationMachine.AppSettings;
using VacationMachine.Database;
using VacationMachine.Database.Schema;
using VacationMachine.Models;
using VacationMachine.ResultHandlers;

namespace VacationMachine;

public class VacationService
{
    private readonly IVacationDatabase _database;
    private readonly List<IResultHandler> _resultHandlers;
    private readonly VacationDaysLimitSettings _vacationDaysLimitSettings;

    public VacationService(
        IVacationDatabase database,
        IEnumerable<IResultHandler> resultHandlers,
        IOptions<VacationDaysLimitSettings> options)
    {
        _database = database;
        _resultHandlers = resultHandlers.ToList();
        _vacationDaysLimitSettings = options.Current;
    }

    public Result RequestPaidDaysOff(RequestModel requestModel)
    {
        if (requestModel.DaysToTake <= 0) throw new ArgumentException();

        Employee employeeData = _database.FindByEmployeeId(requestModel.EmployeeId);
        long daysTakenSoFar = employeeData.ApprovedVacationRequestsList.Sum(a => a.Days);
        long daysLimit = GetDefaultLimitForSpecificEmployeeStatus(employeeData.Status);
        long additionalDaysLimit = GetAdditionalLimitForSpecificEmployeeStatus(employeeData.Status);

        Result result = CalculateResult(daysLimit, additionalDaysLimit, requestModel.DaysToTake, daysTakenSoFar);
        HandleResult(result, employeeData, requestModel.DaysToTake);

        return result;
    }

    private Result CalculateResult(long daysLimit, long additionalDaysLimit, long daysToTake, long daysTakenSoFar)
    {
        if (daysLimit >= daysToTake + daysTakenSoFar)
            return Result.Approved;
        if (daysLimit + additionalDaysLimit >= daysToTake + daysTakenSoFar)
            return Result.Manual;

        return Result.Denied;
    }

    private void HandleResult(Result result, Employee employee, long daysToTake)
    {
        foreach (IResultHandler resultHandler in _resultHandlers)
            resultHandler.Handle(result, employee, daysToTake);
    }

    private long GetDefaultLimitForSpecificEmployeeStatus(Employee.EmployeeStatus employeeStatus)
    {
        return GetValueFromPropertyOrDefault(
            _vacationDaysLimitSettings.Default,
            employeeStatus.ToString(),
            _vacationDaysLimitSettings.Default.Default);
    }

    private long GetAdditionalLimitForSpecificEmployeeStatus(Employee.EmployeeStatus employeeStatus)
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