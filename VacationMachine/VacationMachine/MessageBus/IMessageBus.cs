namespace VacationMachine.MessageBus;

public interface IMessageBus
{
    void SendEvent(string msg);
}