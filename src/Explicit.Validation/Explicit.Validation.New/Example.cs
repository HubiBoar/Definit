namespace Explicit.Validation.New;

public class ExampleValue : ISectionValue<string, IsConnectionString>
{
    public static string SectionName { get; }
}

public class ExampleSection : ISectionName, IValidationMethod<ExampleSection>
{
    public static string SectionName { get; }

    public string Value { get; }

    public static void SetupValidation(ValidationContext<ExampleSection> context)
    {
        context.Add
    }
}

public sealed class IsConnectionString : IValidationMethod<string>
{
    public static void SetupValidation(ValidationContext<string> value)
    {
    }
}

public class Example
{
    private readonly OptionsHolder<ExampleSection> _section;
    private readonly OptionsHolder<string, ExampleValue> _value;

    public Example(OptionsHolder<ExampleSection> section, OptionsHolder<string, ExampleValue> value)
    {
        _section = section;
        _value = value;
    }
    
    private void Test()
    {
        var isValidSection = _section.Get();
        var isValidValue = _value.Get();
    }
}