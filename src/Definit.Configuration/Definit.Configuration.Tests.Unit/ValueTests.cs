using Definit.Primitives;

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
        section.Is(out string valid);

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
        ((bool)section.Is(out ValidationErrors _)).Should().BeTrue();
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