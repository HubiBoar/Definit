using Definit.Results;

namespace Definit.Validation.FluentValidation;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>
    where TValue : notnull
{
    public FluentValidator()
    {
    }
    
    public static ValidationResult Validate(TValue value, Action<FluentValidator<TValue>> setup)
    {
        var validator = new FluentValidator<TValue>();
        setup(validator);
        var result = validator.Validate(value);
        
        if (result.IsValid)
        {
            return Success.Instance;
        }

        var errorsGroup = result.Errors
            .GroupBy(x => x.PropertyName)
            .ToArray();

        if(errorsGroup.Length == 1)
        {
            return new ValidationErrors(errorsGroup
                .ToDictionary(x => ValidationErrors.IgnorePropertyName, x => x
                    .Select(e => e.ErrorMessage)
                    .ToArray()));
        }

        return new ValidationErrors(result.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x
                .Select(e => e.ErrorMessage)
                .ToArray()));
    }
}