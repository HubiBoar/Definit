using Definit.Results;

namespace Definit.Validation;

public static class ValidationHelper
{
    public static ValidationResult Invoke<TValue>(this Validator<TValue>.Delegate validate, TValue value)
    {
        return validate(new Validator<TValue>(value));
    }

    public static ValidationResult InvokeAndReturnValue<TValue>(this Validator<TValue>.Delegate validate, TValue value)
    {
        return validate.Invoke(value);
    }
}