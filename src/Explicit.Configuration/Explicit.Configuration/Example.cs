using Explicit.Validation;
using Explicit.Validation.FluentValidation;

namespace Explicit.Configuration;

public class ExampleConfigSection : IConfigSection<ExampleConfigSection>
{
    public static string SectionName { get; } = "ExampleConfigSection";

    public string Value1 { get; }
    
    public string Value2 { get; }

    public static OneOf<Success, ValidationErrors> Validate(Validator<ExampleConfigSection> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value1).IsConnectionString();

            validator.RuleFor(x => x.Value2).IsConnectionString();
        });
    }
}

public class ExampleConfigValue : IConfigValue<string, IsCommaArrayOf<IsConnectionString>>
{
    public static string SectionName { get; } = "ExampleConfigValue";
}

public class ExampleDependency
{
    public IConfigHolder<ExampleConfigValue> Value { get; }

    public IConfigHolder<ExampleConfigSection> Section { get; }
    
    public ExampleDependency(IConfigHolder<ExampleConfigValue> value, IConfigHolder<ExampleConfigSection> section)
    {
        Value = value;
        Section = section;
    }

    private void Values()
    {
        var value = Value.Get<string, ExampleConfigValue>();
        var section = Section.Get();
    }

}