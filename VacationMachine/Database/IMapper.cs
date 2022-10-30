namespace VacationMachine
{
    public interface IMapper
    {
        public Domain.Employee ToDomain(Business.Employee businessEmployee);

        public Business.Employee ToBusiness(Domain.Employee domainEmployee);
    }
}
