namespace Definit.Validation;

public record Validator<TValue>(TValue Value);

public interface IValidate<TValue>
{
    public static abstract OneOf<Success, ValidationErrors> Validate(Validator<TValue> context);
}