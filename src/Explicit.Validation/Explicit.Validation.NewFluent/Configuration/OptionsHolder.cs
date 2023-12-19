using Explicit.Validation.NewFluent.Primitives;
using Microsoft.Extensions.Configuration;

namespace Explicit.Validation.NewFluent.Configuration;

public interface IOptionsHolder<TSection>
    where TSection : class, ISectionName, IValidate<TSection>
{
    IsValid<TSection> Get();
}

public interface IOptionsHolder<TValue, TSection>
    where TValue : notnull
    where TSection : ISectionName, IValidationRule<TValue>
{
    IsValid<Value<TValue, TSection>> Get();
}

internal sealed class OptionsHolder<TSection> : IOptionsHolder<TSection>
    where TSection : class, ISectionName, IValidate<TSection>
{
    private IConfiguration Configuration { get; }

    public OptionsHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IsValid<TSection> Get()
    {
        var sectionValue = Configuration.GetValue<TSection>(TSection.SectionName)!;

        return IsValid<TSection>.Create(sectionValue);
    }
}

internal sealed class OptionsHolder<TValue, TSection> : IOptionsHolder<TValue, TSection>
    where TValue : notnull
    where TSection : ISectionName, IValidationRule<TValue>
{
    private IConfiguration Configuration { get; }

    public OptionsHolder(IConfiguration configuration)
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