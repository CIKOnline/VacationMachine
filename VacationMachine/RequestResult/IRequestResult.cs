namespace VacationMachine
{
    public interface IRequestResult
    {
        public string Name { get; }

        public void ProcessRequest();
    }
}
