using Explicit.Validation;
using Explicit.Validation.FluentValidation;

namespace Explicit.Configuration;

public interface IExampleConfigSection
{
}

public class ExampleConfigSection : IConfigSection<ExampleConfigSection>
{
    public static string SectionName { get; } = "ExampleConfigSection";

    public string Value1 { get; } = string.Empty;
    
    public string Value2 { get; } = string.Empty;

    public static OneOf<Success, ValidationErrors> Validate(Validator<ExampleConfigSection> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value1).IsConnectionString();

            validator.RuleFor(x => x.Value2).IsConnectionString();
        });
    }
}

public class ExampleConfigValue : IConfigValue<ExampleConfigValue, string, IsCommaArrayOf<IsConnectionString>>
{
    public static string SectionName { get; } = "ExampleConfigValue";

    public string Value { get; init; } = string.Empty;
}

public sealed class ExampleFeatureName : IFeatureName
{
    public static string FeatureName => "ExampleFeatureName";
}

public class ExampleDependency
{
    public IConfigHolder<ExampleConfigValue> Value { get; }

    public IConfigHolder<ExampleConfigSection> Section { get; }

    public IConfigHolder<FeatureToggle<ExampleFeatureName>> Feature { get; }

    public ExampleDependency(
        IConfigHolder<ExampleConfigValue> value,
        IConfigHolder<ExampleConfigSection> section,
        IConfigHolder<FeatureToggle<ExampleFeatureName>> feature)
    {
        Value = value;
        Section = section;
        Feature = feature;
    }

    private void Values()
    {
        var value = Value.GetValid();
        var section = Section.GetValid();
        var enabled = Feature.GetValid();
    }
}