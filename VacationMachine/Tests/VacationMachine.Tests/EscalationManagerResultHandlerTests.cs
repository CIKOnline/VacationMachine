using Moq;
using VacationMachine.Database.Schema;
using VacationMachine.Escalation;
using VacationMachine.ResultHandlers;

namespace VacationMachine.Tests;

public class EscalationManagerResultHandlerTests
{
    [Test]
    public void When_ResultIsManual_Then_EscalationManagerIsInvoked()
    {
        //Arrange
        Mock<IEscalationManager> escalationManagerMock = new();
        var sut = new EscalationManagerResultHandler(escalationManagerMock.Object);
        
        //Act
        sut.Handle(Result.Manual, new Employee(), 0);
        
        //Assert
        escalationManagerMock.Verify(e => e.NotifyNewPendingRequest(It.IsAny<long>()), Times.Once);
    }
    
    [TestCase(Result.Approved)]
    [TestCase(Result.Denied)]
    public void When_ResultIsNotManual_Then_EscalationManagerIsNotInvoked(Result result)
    {
        //Arrange
        Mock<IEscalationManager> escalationManagerMock = new();
        var sut = new EscalationManagerResultHandler(escalationManagerMock.Object);
        
        //Act
        sut.Handle(result, new Employee(), 0);
        
        //Assert
        escalationManagerMock.Verify(e => e.NotifyNewPendingRequest(It.IsAny<long>()), Times.Never);
    }
}