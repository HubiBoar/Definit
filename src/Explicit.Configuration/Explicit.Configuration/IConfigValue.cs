using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explicit.Configuration;

public interface IConfigValue<TSelf, TValue, TMethod> : IConfigObject<TSelf>
    where TSelf : IConfigValue<TSelf, TValue, TMethod>, new()
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    public TValue Value { get; init; }

    static IsValid<TSelf> IConfigObject<TSelf>.GetFromConfiguration(IServiceProvider provider, IConfigurationSection section)
    {
        var value = section.Get<TValue>();
        if (value is null)
        {
            return IsValid<TSelf>.Create(default);
        }
        return new TSelf { Value = value }.IsValid();
    }

    static void IConfigObject<TSelf>.RegisterDepedencies(IServiceCollection services)
    {
    }

    static SectionValue IConfigObject<TSelf>.ConvertToSection(TSelf value)
    {
        return new SectionValue(value.Value);
    }

    static OneOf<Success, ValidationErrors> IValidate<TSelf>.Validate(Validator<TSelf> context)
    {
        return TMethod.Validate(new Validator<TValue>(context.Value.Value));
    }
}