using VacationMachine.AppSettings;
using VacationMachine.Database.Schema;
using VacationMachine.MessageBus;

namespace VacationMachine.ResultHandlers;

public class MessageBusResultHandler : IResultHandler
{
    private readonly IMessageBus _messageBus;
    private readonly IOptions<MessageBusMessageSettings> _messageBusMessageSettings;

    public MessageBusResultHandler(IMessageBus messageBus, IOptions<MessageBusMessageSettings> messageBusMessageSettings)
    {
        _messageBus = messageBus;
        _messageBusMessageSettings = messageBusMessageSettings;
    }

    public void Handle(Result result, Employee employee, long daysToTake)
    {
        var message = _messageBusMessageSettings.Current.GetType()
            .GetProperty(result.ToString())
            ?.GetValue(_messageBusMessageSettings.Current) as string;

        if (message is not null)
            _messageBus.SendEvent(message);
    }
}