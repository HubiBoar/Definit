using Explicit.Configuration.FluentValidation;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OneOf;
using OneOf.Types;

namespace Explicit.Configuration.Tests.Unit;

public class TestSection : IOptionsSection<TestSection>
{
    public static string SectionName { get; } = "test";

    public OneOf<Success, ValidationErrors> Validate()
    {
        return new Success();
    }
}

public class ConfigurationTests
{
    [Fact]
    public void Test1()
    {
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfigurationManager>();
        AddOptionsHelper.AddOptions<TestSection>(services, configuration);
        
        services.Should
    }
}