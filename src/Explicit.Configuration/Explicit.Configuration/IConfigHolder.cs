using Explicit.Primitives;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigHolder<TSection>
    where TSection : ISectionName, IValidate<TSection>
{
    IsValid<TSection> Get();
}

public interface IConfigHolder<TValue, TSection>
    where TValue : notnull
    where TSection : IConfigValue<TValue>, IValidate<TValue>
{
    IsValid<Value<TValue, TSection>> Get();
}

internal sealed class ConfigHolder<TSection> : IConfigHolder<TSection>
    where TSection : ISectionName, IValidate<TSection>
{
    private IConfiguration Configuration { get; }

    public ConfigHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IsValid<TSection> Get()
    {
        var sectionValue = Configuration.GetValue<TSection>(TSection.SectionName)!;

        return IsValid<TSection>.Create(sectionValue);
    }
}

internal sealed class ConfigHolder<TValue, TSection> : IConfigHolder<TValue, TSection>
    where TValue : notnull
    where TSection : IConfigValue<TValue>, IValidate<TValue>
{
    private IConfiguration Configuration { get; }

    public ConfigHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IsValid<Value<TValue, TSection>> Get()
    {
        var sectionValue = Configuration.GetValue<TValue>(TSection.SectionName)!;

        var value = new Value<TValue, TSection>(sectionValue);
        
        return IsValid<Value<TValue, TSection>>.Create(value);
    }
}