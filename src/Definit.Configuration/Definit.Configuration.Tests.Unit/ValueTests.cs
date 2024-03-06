namespace Definit.Configuration.Tests.Unit;

public class ValueTests
{
    [Fact]
    public void GetFromConfigTest()
    {
        //Arrange
        var values = new Dictionary<string, string>
        {
            {"testValue", "TestValue"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = TestValue.Create(configuration);

        //Assert
        var valid = section.AsT0.ValidValue.GetValue();

        valid.Should().Be("TestValue");
    }
    
    [Fact]
    public void GetNullFromConfigTest()
    {
        //Arrange
        var values = new Dictionary<string, string> { };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = TestValue.Create(configuration);

        //Assert
        section.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public void RegisterTest()
    {
        //Arrange
        var services = new ServiceCollection();
        var values = new Dictionary<string, string>
        {
            {"testValue", "TestValue"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        TestValue.Register(services, configuration);

        //Assert
        services.Should().Contain(x => x.ServiceType == typeof(TestValue.Get));
    }
}