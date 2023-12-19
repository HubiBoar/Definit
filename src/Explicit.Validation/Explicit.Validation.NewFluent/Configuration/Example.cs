using Explicit.Validation.NewFluent.Fluent;

namespace Explicit.Validation.NewFluent.Configuration;

public class ExampleConfigSection : IConfigSection<ExampleConfigSection>
{
    public static string SectionName { get; } = "ExampleConfigSection";

    public string Value1 { get; }
    
    public string Value2 { get; }

    public static void SetupValidation(FluentValidator<ExampleConfigSection> validator)
    {
        validator.RuleFor(x => x.Value1).IsConnectionString();

        validator.RuleFor(x => x.Value2).IsConnectionString();
    }
}

public class ExampleConfigValue : IConfigValue<string, IsCommaArrayOf<IsConnectionString>>
{
    public static string SectionName { get; } = "ExampleConfigValue";
}