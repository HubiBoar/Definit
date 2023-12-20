using Explicit.Validation;
using OneOf;
using OneOf.Types;

namespace Explicit.Configuration.Tests.Unit;

public class TestSection : IConfigSection<TestSection>
{
    public static string SectionName { get; } = "testSection";

    public string Value0 { get; init; }
    
    public string Value1 { get; init; }

    public static OneOf<Success, ValidationErrors> Validate(Validator<TestSection> context)
    {
        return new Success();
    }
}

public class TestValue : IConfigValue<string, TestValue>
{
    public static string SectionName { get; } = "testValue";

    public static OneOf<Success, ValidationErrors> Validate(Validator<string> context)
    {
        return new Success();
    }
}