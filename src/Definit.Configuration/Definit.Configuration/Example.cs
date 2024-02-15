namespace Definit.Configuration;

internal static class Example
{
    private sealed class Feature : IFeatureName
    {
        public static string FeatureName => "Test";
    }

    private sealed class Section : ConfigSection<Section>
    {
        protected override string SectionName { get; } = "ExampleConfigSection";

        public string Value1 { get; } = string.Empty;
        
        public string Value2 { get; } = string.Empty;

        protected override OneOf<Success, ValidationErrors> Validate(Validator<Section> context)
        {
            return context.Fluent(validator =>
            {
                validator.RuleFor(x => x.Value1).IsConnectionString();

                validator.RuleFor(x => x.Value2).IsConnectionString();
            });
        }
    }

    private sealed class Value : ConfigValue<Value, string, IsConnectionString>
    {
        protected override string SectionName => "Name";
    }



    private static async Task Get(Section.Get sectionGetter, Value.Get valueGetter, FeatureToggle<Feature>.Get featureGetter)
    {
        var section = sectionGetter();
        var value = valueGetter();
        var isEnabled = await featureGetter();
    }
   
    private static void Register(IServiceCollection services, IConfiguration configuration)
    {
        Section.Register(services, configuration);
        Value.Register(services, configuration);
        FeatureToggle<Feature>.Register(services);
    }

    private static async Task Create(IServiceCollection services, IConfiguration configuration)
    {
        var section = Section.Create(configuration);
        var value = Value.Create(configuration);
        var isEnabled = await FeatureToggle<Feature>.Create(services);
    }
}