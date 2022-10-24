namespace VacationMachine.Tests;

public class IoCProviderTests
{
    [Test]
    public void When_TypeIsRegisteredAsTransient_Then_ItCanBeResolved()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterTransient<ITestInterface, TestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject = provider.Get<ITestInterface>();
        //Assert
        Assert.IsInstanceOf<TestObject>(returnedObject);
    }

    [Test]
    public void When_TypeIsRegisteredAsSingleton_Then_ItCanBeResolved()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<ITestInterface, TestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject = provider.Get<ITestInterface>();
        //Assert
        Assert.IsInstanceOf<TestObject>(returnedObject);
    }

    [Test]
    public void When_TypeIsRegisteredAsTransient_Then_OutputIsAlwaysDifferent()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterTransient<ITestInterface, TestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject1 = provider.Get<ITestInterface>();
        var returnedObject2 = provider.Get<ITestInterface>();
        //Assert
        Assert.That(returnedObject2, Is.Not.SameAs(returnedObject1));
    }


    [Test]
    public void When_TypeIsRegisteredAsSingleton_Then_OutputIsAlwaysTheSame()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<ITestInterface, TestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject1 = provider.Get<ITestInterface>();
        var returnedObject2 = provider.Get<ITestInterface>();
        //Assert
        Assert.That(returnedObject2, Is.SameAs(returnedObject1));
    }

    [Test]
    public void When_TypeWithDependenciesIsRegistered_Then_ItCanBeResolved()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<ITestInterface, TestObject>();
        container.RegisterSingleton<SecondTestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject = provider.Get<SecondTestObject>();
        //Assert
        Assert.That(returnedObject, Is.Not.Null);
    }

    [Test]
    public void When_NotAllDependenciesAreRegistered_Then_ExceptionIsThrown()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<SecondTestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        Assert.Catch(() => provider.Get<SecondTestObject>());
    }

    [Test]
    public void When_CircularDependencyIsDetected_Then_ExceptionIsThrown()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<ThirdTestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        Assert.Catch(() => provider.Get<ThirdTestObject>());
    }

    [Test]
    public void When_ConstructorHasTypedIEnumerable_Then_ItIsSolvedCorrectly()
    {
        //Arrange
        var container = new IoCContainer();
        container.RegisterSingleton<ITestInterface, TestObject>();
        container.RegisterSingleton<FourthTestObject>();
        IoCProvider provider = container.BuildProvider();
        //Act
        var returnedObject = provider.Get<FourthTestObject>();
        //Assert
        Assert.That(returnedObject, Is.Not.Null);
    }
}