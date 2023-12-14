namespace Explicit.Validation.Abstraction;

public interface IValidationMethod<in TValue>
{
    public static abstract OneOf<Success, ValidationErrors> Validate(TValue value);
}