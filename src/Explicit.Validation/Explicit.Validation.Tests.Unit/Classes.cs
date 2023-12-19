namespace Explicit.Validation.Tests.Unit;

internal sealed class ValidatableSuccessClass : IValidate<ValidatableSuccessClass>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<ValidatableSuccessClass> context)
    {
        return new Success();
    }
}

internal sealed class ValidatableErrorClass : IValidate<ValidatableErrorClass>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<ValidatableErrorClass> context)
    {
        return new ValidationErrors();
    }
}

internal sealed class ValidatableSuccessMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> context)
    {
        return new Success();
    }
}

internal sealed class ValidatableErrorMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> context)
    {
        return new ValidationErrors();
    }
}