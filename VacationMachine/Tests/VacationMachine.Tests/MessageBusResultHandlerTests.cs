using Moq;
using VacationMachine.AppSettings;
using VacationMachine.Database.Schema;
using VacationMachine.MessageBus;
using VacationMachine.ResultHandlers;

namespace VacationMachine.Tests;

public class MessageBusResultHandlerTests
{
    private const string CorrectMessageWhenRequestApproved = "Lorem ipsum";
    private readonly MessageBusMessageSettings _settings = new()
    {
        Approved = CorrectMessageWhenRequestApproved,
        Denied = null,
        Manual = null
    };

    private Mock<IOptions<MessageBusMessageSettings>> _optionsMock = null!;
    private Mock<IMessageBus> _messageBusMock = null!;
    private MessageBusResultHandler _sut = null!;

    [SetUp]
    public void Setup()
    {
        _messageBusMock = new Mock<IMessageBus>();
        _optionsMock = new Mock<IOptions<MessageBusMessageSettings>>();
        _optionsMock.Setup(o => o.Current)
            .Returns(_settings);
        
        _sut = new MessageBusResultHandler(_messageBusMock.Object, _optionsMock.Object);

    }
    
    [Test]
    public void When_RequestIsApproved_Then_CorrectMessageIsSent()
    {
        //Act
        _sut.Handle(Result.Approved, new Employee(), 0);
        
        //Assert
        _messageBusMock.Verify(e => e.SendEvent(CorrectMessageWhenRequestApproved), Times.Once);
    }
    
    [TestCase(Result.Denied)]
    [TestCase(Result.Manual)]
    public void When_RequestIsNotApproved_Then_EmailIsNotSent(Result result)
    { 
        //Act
        _sut.Handle(result, new Employee(), 0);
        
        //Assert
        _messageBusMock.Verify(e => e.SendEvent(It.IsAny<string>()), Times.Never);
    }
}