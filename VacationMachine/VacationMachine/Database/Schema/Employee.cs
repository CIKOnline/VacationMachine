using System.Collections.Generic;

namespace VacationMachine.Database.Schema;

public class Employee
{
    public enum EmployeeStatus
    {
        Performer,
        Regular,
        Slacker
    }

    public long EmployeeId { get; init; }

    public EmployeeStatus Status { get; set; }

    public List<ApprovedVacationRequests> ApprovedVacationRequestsList { get; set; } = new();
}