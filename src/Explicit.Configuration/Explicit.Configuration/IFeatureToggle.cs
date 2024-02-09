namespace Explicit.Configuration;

public interface IFeatureName
{
    public static abstract string FeatureName { get; }    
}

public sealed class FeatureToggle<T> : ISectionName
    where T : IFeatureName
{
    public delegate Task<bool> Get();

    public static string SectionName => $"FeatureManagement:{T.FeatureName}";

    public static void Register(IServiceCollection services)
    {
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
}