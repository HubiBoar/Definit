using Definit.Results;

namespace Definit.Validation;

public sealed record Validator<TValue>(TValue Value)
{
    public delegate ValidationResult Delegate(Validator<TValue> context);
}

public interface IValidate<TValue>
{
    public static abstract ValidationResult Validate(Validator<TValue> context);
}