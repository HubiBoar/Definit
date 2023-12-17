using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneOf.Else;

namespace Explicit.Configuration;

public static class AddOptionsHelper
{
    public static void AddOptions<TOptions>(
        IServiceCollection services,
        IConfigurationManager configuration)
            where TOptions : class, IOptionsObject, IOptionsConfiguration<TOptions>
    {
        var builder = services.AddOptions<TOptions>();
        var section = configuration.GetRequiredSection(TOptions.SectionName);
        TOptions.Configure(builder, section);

        services.AddSingleton<IOptionsHolder<TOptions>, OptionsHolder<TOptions>>();
    }
    
    public static OneOf<TOptions, ValidationErrors, Exception> GetOptions<TOptions>(IServiceCollection services)
        where TOptions : class, IOptionsObject
    {
        try
        {
            using var provider = services.BuildServiceProvider();
            var holder = provider.GetRequiredService<IOptionsHolder<TOptions>>();

            return holder.GetValue().Basic.As<TOptions, ValidationErrors, Exception>();
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