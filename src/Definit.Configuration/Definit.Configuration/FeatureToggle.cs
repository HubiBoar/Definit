namespace Definit.Configuration;

public interface IFeatureName
{
    public static abstract string FeatureName { get; }    
}

public sealed class FeatureToggle<T> : IConfigObject<Value<bool, IsNotNull<bool>>>, ISectionName
    where T : IFeatureName
{
    public static string SectionName { get; } = $"FeatureManagement:{FeatureName}";
    private readonly static string FeatureName = T.FeatureName;

    public delegate Task<bool> Get();

    public static ValidationResult Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFeatureManagement();
        services.AddSingleton<Get>(provider => () => Create(provider));

        return ValidateConfiguration(configuration);
    }

    public static ValidationResult ValidateConfiguration(IConfiguration configuration)
    {
        try
        {
            var featureName = SectionName;
            var section = configuration.GetSection(featureName);

            if(section is null)
            {
                return new ValidationErrors(SectionName, $"Missing FeatureToggle");
            }

            return ValidationResult.Success;
        }
        catch(Exception exception)
        {
            return exception;
        }
    }

    public static Task<bool> Create(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        return Create(scope.ServiceProvider);
    }

    public static Task<bool> Create(IServiceProvider provider)
    {
        return provider
            .GetRequiredService<IFeatureManager>()
            .IsEnabledAsync(FeatureName);
    }

    public static IsValid<Value<bool, IsNotNull<bool>>> Create(IServiceProvider provider, IConfiguration _)
    {
        return Create(provider)
            .GetAwaiter()
            .GetResult()
            .IsValid<bool, IsNotNull<bool>>();
    }
}