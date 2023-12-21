using Explicit.Validation;
using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigValue<TSelf, TValue, TMethod> : IConfigObject<TSelf>
    where TSelf : IConfigValue<TSelf, TValue, TMethod>, new()
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    public TValue Value { get; init; }

    static IsValid<TSelf> IConfigObject<TSelf>.CreateSection(IConfigurationSection section)
    {
        var value = section.Get<TValue>()!;
        return new TSelf { Value = value }.IsValid();
    }

    static OneOf<Success, ValidationErrors> IValidate<TSelf>.Validate(Validator<TSelf> context)
    {
        return TMethod.Validate(new Validator<TValue>(context.Value.Value));
    }
}