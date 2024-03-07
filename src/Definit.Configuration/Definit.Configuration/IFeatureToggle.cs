
namespace Definit.Configuration;

public interface IFeatureName
{
    public static abstract string FeatureName { get; }    
}

public sealed class FeatureToggle<T> : ISectionName, IConfigObject
    where T : IFeatureName
{
    public delegate Task<bool> Get();

    public static string SectionName => $"FeatureManagement:{T.FeatureName}";

    public static void Register(IServiceCollection services)
    {
        services.AddFeatureManagement();
        services.AddSingleton<Get>(provider => () => Create(provider));
    }

    public static Task<bool> Create(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        
        return Create(scope.ServiceProvider);
    }

    public static async Task<bool> Create(IServiceProvider provider)
    {
        return await provider
            .GetRequiredService<IFeatureManager>()
            .IsEnabledAsync(T.FeatureName);
    }

    public static OneOf<Success, ValidationErrors> ValidateConfiguration(IConfiguration configuration)
    {
        var featureName = SectionName;
        var section = configuration.GetSection(featureName);

        if(section is null)
        {
            return new ValidationErrors($"Missing FeatureToggle :: [{featureName}]");
        }

        return new Success();
    }
}