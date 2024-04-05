namespace Definit.Configuration.Tests.Unit;

public sealed class TestSection : ConfigSection<TestSection>
{
    protected override string SectionName { get; } = "testSection";

    public string Value0 { get; init; } = string.Empty;
    
    public string Value1 { get; init; } = string.Empty;

    protected override ValidationResult Validate(Validator<TestSection> context)
    {
        return ValidationResult.Success;
    }
}

public sealed class TestValidation : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return ValidationResult.Success;
    }
}

public sealed class TestValue : ConfigValue<TestValue, string, TestValidation>
{
    protected override string SectionName { get; } = "testValue";
}