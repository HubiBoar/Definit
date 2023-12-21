using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Explicit.Configuration;

public static class AddConfigExtensions
{
    public static void AddConfig<TConfig>(this IServiceCollection services, IConfiguration configuration)
        where TConfig : IConfigObject<TConfig>
    {
        services.TryAddSingleton(new ConfigurationHolder(configuration));
        services.AddSingleton<IConfigHolder<TConfig>, ConfigHolder<TConfig>>();
    }
    
    public static IsValid<TSection> GetValid<TSection>(this IConfiguration configuration)
        where TSection : IConfigObject<TSection>
    {
        return TSection.GetFromConfiguration(configuration.GetSection(TSection.SectionName));
    }
}