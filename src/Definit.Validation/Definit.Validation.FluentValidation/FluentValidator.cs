namespace Definit.Validation.FluentValidation;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>
    where TValue : notnull
{
    private FluentValidator()
    {
    }
    
    public static OneOf<Success, ValidationErrors> Validate(TValue value, Action<FluentValidator<TValue>> setup)
    {
        var validator = new FluentValidator<TValue>();
        setup(validator);
        var result = validator.Validate(value);
        
        if (result.IsValid)
        {
            return new Success();
        }

        var errors = new ValidationErrors(result.Errors.Select(x => x.ErrorMessage).ToArray());

        return errors;
    }
}