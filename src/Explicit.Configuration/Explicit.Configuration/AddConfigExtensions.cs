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
        services.TryAddSingleton(provider => new ConfigurationHolder(provider, configuration));
        services.AddSingleton<IConfigHolder<TConfig>, ConfigHolder<TConfig>>();
        TConfig.RegisterDepedencies(services);
    }
    
    public static IsValid<TSection> GetValid<TSection>(this IConfiguration configuration, IServiceCollection services)
        where TSection : IConfigObject<TSection>
    {
        using var scope = services.BuildServiceProvider().CreateScope();

        var section = configuration.GetSection(TSection.SectionName);
        if(section.Exists() == false)
        {
            return IsValid<TSection>.Error(new ValidationErrors($"Section: [{TSection.SectionName}] Is Missing"));
        }

        return TSection.GetFromConfiguration(scope.ServiceProvider, section);
    }
}