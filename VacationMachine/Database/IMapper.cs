namespace VacationMachine
{
    public interface IEmployeeMapper
    {
        public Domain.Employee ToDomain(Business.Employee businessEmployee);

        public Business.Employee ToBusiness(Domain.Employee domainEmployee);
    }
}
