using Microsoft.Extensions.Configuration;
using OneOf;
using OneOf.Types;

namespace Explicit.Validation.New;

public sealed class Value<TValue, TMethod> : IValidationMethod<Value<TValue, TMethod>>
    where TValue : notnull
    where TMethod : IValidationMethod<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue()
    {
        return _value;
    }

    public static OneOf<Success, ValidationErrors> SetupValidation(ValidationContext<Value<TValue, TMethod>> value)
    {
        return ValidationContext<TValue>.IsValid(value);
    }
}

public interface ISectionValue<TValue, TMethod> : ISectionName, IValidationMethod<TValue>
    where TValue : notnull
    where TMethod :  IValidationMethod<TValue>
{
    static void IValidationMethod<TValue>.SetupValidation(ValidationContext<TValue> value)
    {
        TMethod.SetupValidation(value);
    }
}

public interface ISectionName
{
    public static abstract string SectionName { get; }
}

public sealed class OptionsHolder<TSection>
    where TSection : ISectionName, IValidationMethod<TSection>
{
    private IConfiguration Configuration { get; }

    public OptionsHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IsValid<TSection> Get()
    {
        var sectionValue = Configuration.GetValue<TSection>(TSection.SectionName)!;

        return ValidationContext<TSection>.IsValid<TSection>(sectionValue);
    }
}

public sealed class OptionsHolder<TValue, TSection>
    where TValue : notnull
    where TSection : ISectionName, IValidationMethod<TValue>
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
        
        return ValidationContext<Value<TValue, TSection>>.IsValid<Value<TValue, TSection>>(value);
    }
}