using Definit.Results;

namespace Definit.Validation.Tests.Unit.Validation;

internal sealed class ValidatableSuccessClass : IValidate<ValidatableSuccessClass>
{
    public static ValidationResult Validate(Validator<ValidatableSuccessClass> context)
    {
        return Success.Instance;
    }
}

internal sealed class ValidatableErrorClass : IValidate<ValidatableErrorClass>
{
    public static ValidationResult Validate(Validator<ValidatableErrorClass> context)
    {
        return new ValidationErrors("Example", "Example");
    }
}

internal sealed class ValidatableSuccessMethod : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return Success.Instance;
    }
}

internal sealed class ValidatableErrorMethod : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return new ValidationErrors("Example", "Example");
    }
}