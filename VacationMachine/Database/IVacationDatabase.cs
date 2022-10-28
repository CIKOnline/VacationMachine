namespace VacationMachine
{
    public interface IVacationDatabase
    {
        Employee FindByEmployeeId(long employeeId);

        void Save(Employee employeeData);
    }
}
