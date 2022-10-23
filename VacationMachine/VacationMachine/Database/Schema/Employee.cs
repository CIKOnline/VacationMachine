using System.Collections.Generic;

namespace VacationMachine.Database.Schema;

public class Employee
{
    public long EmployeeId { get; set; }
    
    public EmployeeStatus Status { get; set; }

    public List<ApprovedVacationRequests> ApprovedVacationRequestsList { get; set; }

    public enum EmployeeStatus
    {
        Performer,
        Regular,
        Slacker
    }
}