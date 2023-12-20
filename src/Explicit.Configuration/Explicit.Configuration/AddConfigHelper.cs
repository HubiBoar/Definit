using Explicit.Primitives;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Explicit.Configuration;

public static class AddConfigHelper
{
    public static void AddConfig<TConfig>(
        this IServiceCollection services,
        IConfiguration configuration)
            where TConfig : ISectionName
    {
        services.TryAddSingleton(configuration);
        services.AddSingleton<IConfigHolder<TConfig>, ConfigHolder<TConfig>>();
    }
    
    public static IsValid<TSection> GetSection<TSection>(this IConfigHolder<TSection> holder)
        where TSection : IConfigSection<TSection>
    {
        return holder.Configuration.GetValid<TSection>();
    }

    public static IsValid<Value<TValue, TSection>> GetValue<TValue, TSection>(this IConfigHolder<TSection> holder)
        where TSection : IConfigValue<TValue>, IValidate<TValue>
        where TValue : notnull
    {
        return holder.Configuration.GetValid<TValue, TSection>();
    }
    
    public static IsValid<TSection> GetValid<TSection>(this IConfiguration configuration)
        where TSection : IConfigSection<TSection>
    {
        var sectionValue = configuration.GetSection(TSection.SectionName).Get<TSection>()!;

        return IsValid<TSection>.Create(sectionValue);
    }

    public static IsValid<Value<TValue, TSection>> GetValid<TValue, TSection>(this IConfiguration configuration)
        where TSection : IConfigValue<TValue>, IValidate<TValue>
        where TValue : notnull
    {
        var sectionValue = configuration.GetSection(TSection.SectionName).Get<TValue>()!;

        return IsValid<Value<TValue, TSection>>.Create(sectionValue);
    }
}