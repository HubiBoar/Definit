using Explicit.Validation;

namespace Explicit.Configuration;

public interface IConfigValue<TValue, TMethod> : ISectionName, IValidate<TValue>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    static OneOf<Success, ValidationErrors> IValidate<TValue>.Validate(Validator<TValue> context)
    {
        return TMethod.Validate(context);
    }
}