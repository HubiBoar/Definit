namespace Explicit.Validation.Tests.Unit;

internal sealed class ValidatableSuccessClass : IValidatable
{
    public OneOf<Success, ValidationErrors> Validate()
    {
        return new Success();
    }
}

internal sealed class ValidatableErrorClass : IValidatable
{
    public OneOf<Success, ValidationErrors> Validate()
    {
        return new ValidationErrors();
    }
}

internal sealed class ValidatableSuccessMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(string value)
    {
        return new Success();
    }
}

internal sealed class ValidatableErrorMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(string value)
    {
        return new ValidationErrors();
    }
}