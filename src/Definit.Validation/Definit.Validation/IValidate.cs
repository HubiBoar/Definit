namespace Definit.Validation;

public sealed record Validator<TValue>(TValue Value)
{
    public delegate OneOf<Success, ValidationErrors> Delegate(Validator<TValue> context);
}

public interface IValidate<TValue>
{
    public static abstract OneOf<Success, ValidationErrors> Validate(Validator<TValue> context);
}