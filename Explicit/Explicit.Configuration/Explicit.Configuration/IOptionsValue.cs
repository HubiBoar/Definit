using Explicit.Utils;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Explicit.Configuration;

public interface IHoldValue<TValue>
{
    public TValue Value { get; init; }
}

public interface IOptionsValueBase<TValue> : IOptionsObject, IHoldValue<TValue>, IOptionsConfiguration<IHoldValue<TValue>>
    where TValue : notnull
{
    static void IOptionsConfiguration<IHoldValue<TValue>>.Configure<TOptions>(
        OptionsBuilder<TOptions> configure,
        IConfigurationSection configuration)
    {
        var value = configuration.Get<TValue>() ?? throw new Exception($"Missing Options: {typeof(TOptions).GetTypeVerboseName()}");
       
        var options = Activator.CreateInstance<TOptions>();
    
        typeof(TOptions).GetProperty(nameof(Value))!.SetValue(options, value);
    }

    void IOptionsObject.SetValue(IConfigurationSection configuration)
    {
        var value = configuration.Get<TValue>();
        GetType().GetProperty(nameof(Value))!.SetValue(this, value);
    }
}

public interface IOptionsValue<TValue, TMethod> : IOptionsValueBase<TValue>
    where TValue : notnull
    where TMethod : IValidationMethod<TValue>
{
    SectionValue IOptionsObject.ConvertToSection()
    {
        return new SectionValue(Value);
    }

    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        if (Value is null)
        {
            return new ValidationErrors("Value is missing");
        }
        return TMethod.Validate(Value);
    }
}