namespace VacationMachine.Services.MessageBus
{
    public interface IMessageBus
    {
        void SendEvent(string msg);
    }
}
