using Moq;
using ThirdPartyLibraries.Email;
using VacationMachine.AppSettings;
using VacationMachine.BusinessLogic.ResultHandlers.Concrete;
using VacationMachine.DataAccess.DataModels;

namespace VacationMachine.Tests;

public class SendEmailResultResultHandlerTests
{
    private const string CorrectEmailTextWhenDenied = "Lorem ipsum";

    private readonly EmailMessageSettings _settings = new()
    {
        Approved = null,
        Denied = CorrectEmailTextWhenDenied,
        Manual = null
    };

    private Mock<IEmailSender> _emailSenderMock = null!;
    private Mock<IOptions<EmailMessageSettings>> _optionsMock = null!;
    private SendEmailResultResultHandler _sut = null!;

    [SetUp]
    public void Setup()
    {
        _emailSenderMock = new Mock<IEmailSender>();
        _optionsMock = new Mock<IOptions<EmailMessageSettings>>();
        _optionsMock.Setup(o => o.Current)
            .Returns(_settings);

        _sut = new SendEmailResultResultHandler(_emailSenderMock.Object, _optionsMock.Object);
    }

    [Test]
    public void When_RequestIsRejected_Then_CorrectEmailIsSent()
    {
        //Act
        _sut.Handle(Result.Denied, new Employee(), 0);

        //Assert
        _emailSenderMock.Verify(e => e.Send(CorrectEmailTextWhenDenied), Times.Once);
    }

    [TestCase(Result.Approved)]
    [TestCase(Result.Manual)]
    public void When_RequestIsNotRejected_Then_EmailIsNotSent(Result result)
    {
        //Act
        _sut.Handle(result, new Employee(), 0);

        //Assert
        _emailSenderMock.Verify(e => e.Send(It.IsAny<string>()), Times.Never);
    }
}