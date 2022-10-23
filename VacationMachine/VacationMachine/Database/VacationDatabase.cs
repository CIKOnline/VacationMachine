namespace VacationMachine.Database;

public class VacationDatabase : IVacationDatabase
{
    public object[] FindByEmployeeId(long employeeId)
    {
        return new object[] { "SLACKER", 1 };
    }

    public void Save(object[] employeeData)
    {
    }
}