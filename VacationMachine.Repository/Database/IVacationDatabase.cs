namespace VacationMachine.Repository.Database
{
    public interface IVacationDatabase
    {
        object[] FindByEmployeeId(long employeeId);
        void Save(object[] employeeData);
    }
}
