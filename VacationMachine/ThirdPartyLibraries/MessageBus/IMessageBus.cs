namespace ThirdPartyLibraries.MessageBus;

public interface IMessageBus
{
    void SendEvent(string msg);
}