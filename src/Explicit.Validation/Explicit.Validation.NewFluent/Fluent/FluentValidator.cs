namespace Explicit.Validation.NewFluent.Fluent;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>
    where TValue : IValidate<TValue>
{
    private FluentValidator()
    {
    }

    internal static IsValid<TValue> IsValid(TValue value)
    {
        var validator = new FluentValidator<TValue>();
        var result = validator.Validate(value);
        
        if (result.IsValid)
        {
            return new IsValid<TValue>(new Valid<TValue>(value));
        }

        var errors = new ValidationErrors(result.Errors.Select(x => x.ErrorMessage).ToArray());

        return new IsValid<TValue>(errors);
    }
}
