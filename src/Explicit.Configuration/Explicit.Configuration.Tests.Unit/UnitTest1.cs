using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OneOf;
using OneOf.Types;

namespace Explicit.Configuration.Tests.Unit;

public class TestSection : IConfigSection<TestSection>
{
    public static string SectionName { get; } = "test";

    public static OneOf<Success, ValidationErrors> Validate(Validator<TestSection> context)
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
        AddConfigHelper.AddConfig<TestSection>(services, configuration);
    }
}