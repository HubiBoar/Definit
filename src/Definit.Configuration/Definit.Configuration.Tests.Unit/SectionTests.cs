namespace Definit.Configuration.Tests.Unit;

public class SectionTests
{
    [Fact]
    public void GetFromConfigTest()
    {
        //Arrange
        var values = new Dictionary<string, string>
        {
            {"testSection:Value0", "Value0"},
            {"testSection:Value1", "Value1"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = TestSection.Create(configuration);
       
        //Assert
        var valid = section.AsT0.ValidValue;
        valid.Value0.Should().Be("Value0");
        valid.Value1.Should().Be("Value1");
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
        var section = TestSection.Create(configuration);

        //Assert
        section.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public void GetNullValueFromConfigTest()
    {
        //Arrange
        var values = new Dictionary<string, string>
        {
            {"testSection", ""},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        var section = TestSection.Create(configuration);

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
            {"test:Value0", "Value0"},
            {"test:Value1", "Value1"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(values!)
            .Build();

        //Act
        TestSection.Register(services, configuration);

        //Assert
        services.Should().Contain(x => x.ServiceType == typeof(TestSection.Get));
    }
}