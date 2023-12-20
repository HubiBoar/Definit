using Explicit.Validation;

namespace Explicit.Configuration;

public interface IConfigValue<TValue> : ISectionName
{
}

public interface IConfigValue<TValue, TMethod> : IValidate<TValue>, IConfigValue<TValue>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    static OneOf<Success, ValidationErrors> IValidate<TValue>.Validate(Validator<TValue> context)
    {
        return TMethod.Validate(context);
    }
}