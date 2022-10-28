namespace VacationMachine
{
    public interface IVacationDatabase
    {
        object[] FindByEmployeeId(long employeeId);

        void Save(object[] employeeData);
    }
}
