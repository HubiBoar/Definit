using Explicit.Validation.NewFluent.Fluent;
using Microsoft.Extensions.Configuration;

namespace Explicit.Validation.NewFluent.Configuration;

public interface ISectionValue<TValue, TMethod> : ISectionName, IValidationRule<TValue>
    where TValue : notnull
    where TMethod : IValidationRule<TValue>
{
    static void IValidationRule<TValue>.SetupRule<TFrom>(IRuleBuilder<TFrom, TValue> ruleBuilder)
    {
        TMethod.SetupRule(ruleBuilder);
    }
}

public interface ISectionName
{
    public static abstract string SectionName { get; }
}

public sealed class OptionsHolder<TSection>
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

        return FluentValidator<TSection>.IsValid(sectionValue);
    }
}

public sealed class OptionsHolder<TValue, TSection>
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
        
        return FluentValidator<Value<TValue, TSection>>.IsValid(value);
    }
}