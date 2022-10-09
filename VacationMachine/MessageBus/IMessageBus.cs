namespace VacationMachine
{
    public interface IMessageBus
    {
        void SendEvent(string msg);
    }
}
