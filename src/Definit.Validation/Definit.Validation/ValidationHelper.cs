namespace Definit.Validation;

public static class ValidationHelper
{
    public static OneOf<Success, ValidationErrors> Invoke<TValue>(this Validator<TValue>.Delegate validate, TValue value)
    {
        return validate(new Validator<TValue>(value));
    }

    public static OneOf<TValue, ValidationErrors> InvokeAndReturnValue<TValue>(this Validator<TValue>.Delegate validate, TValue value)
    {
        return validate.Invoke(value).Match<OneOf<TValue, ValidationErrors>>(
            _ => value,
            error => error);
    }
}