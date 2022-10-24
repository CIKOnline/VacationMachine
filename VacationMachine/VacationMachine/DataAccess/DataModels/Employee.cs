using System.Collections.Generic;
using VacationMachine.DataAccess.DataModels.Enums;

namespace VacationMachine.DataAccess.DataModels;

public class Employee
{
    public long EmployeeId { get; init; }

    public EmployeeStatus Status { get; set; }

    public List<ApprovedVacationRequests> ApprovedVacationRequestsList { get; set; } = new();
}