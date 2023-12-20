using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explicit.Configuration.Tests.Unit;

public class ValueTests
{
    [Fact]
    public void GetFromConfigTest()
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
        services.AddConfig<TestValue>(configuration);
        
        //Assert
        var section = configuration.GetValid<string, TestValue>();
        var valid = (string)section.AsT0.ValidValue;

        valid.Should().Be("TestValue");
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
        services.AddConfig<TestValue>(configuration);

        //Assert
        services.Should().Contain(x => x.ServiceType == typeof(IConfigHolder<TestValue>));
    }
}