using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Explicit.Configuration;

public interface IFeatureName
{
    public static abstract string FeatureName { get; }    
}

public sealed class FeatureToggle<T> : IConfigObject<FeatureToggle<T>>
    where T : IFeatureName
{
    public static string SectionName => $"FeatureManagement:{T.FeatureName}";

    public bool IsEnabled { get; }

    private FeatureToggle(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    static void IConfigObject<FeatureToggle<T>>.RegisterDepedencies(IServiceCollection services)
    {
        services.AddFeatureManagement();
    }

    public static SectionValue ConvertToSection(FeatureToggle<T> value)
    {
        return new SectionValue(value.IsEnabled);
    }

    public static IsValid<FeatureToggle<T>> GetFromConfiguration(IServiceProvider provider, IConfigurationSection section)
    {
        var isEnabled = provider
            .GetRequiredService<IFeatureManager>()
            .IsEnabledAsync(T.FeatureName)
            .GetAwaiter()
            .GetResult();

        return new FeatureToggle<T>(isEnabled).IsValid();
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<FeatureToggle<T>> context)
    {
        return new Success();
    }
}