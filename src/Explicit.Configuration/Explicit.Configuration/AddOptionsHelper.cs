using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OneOf.Else;

namespace Explicit.Configuration;

public static class AddOptionsHelper
{
    public static void AddConfig<TConfig>(
        IServiceCollection services,
        IConfigurationManager configuration)
            where TConfig : IConfig
    {
        TConfig.Register(services, configuration);
    }
    
    public static OneOf<TSection, ValidationErrors, Exception> GetConfigSection<TSection>(IServiceCollection services)
        where TSection : class, IConfigSection<TSection>
    {
        try
        {
            using var provider = services.BuildServiceProvider();
            var holder = provider.GetRequiredService<IConfigHolder<TSection>>();

            return holder.Get().Basic.As<TSection, ValidationErrors, Exception>();
        }
        catch (Exception exception)
        {
            return exception;
        }
    }
    
    public static IsValid<TSection> GetOptions<TSection>(this IConfiguration configuration)
        where TSection : class, IOptionsObject
    {
        var options = Activator.CreateInstance<TSection>();
        var section = configuration.GetRequiredSection(TSection.SectionName);
        options.SetValue(section);

        return options.IsValid();
    }
}