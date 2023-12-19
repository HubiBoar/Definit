using Explicit.Validation;

namespace Explicit.Configuration;

public interface IConfigValue<TValue> : ISectionName
{
    
}

public interface IConfigValue<TValue, TMethod> : IConfigValue<TValue>, IValidate<TValue>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    static OneOf<Success, ValidationErrors> IValidate<TValue>.Validate(Validator<TValue> context)
    {
        return TMethod.Validate(context);
    }
}