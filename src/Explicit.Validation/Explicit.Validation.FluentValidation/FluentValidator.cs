using MoreLinq.Extensions;

namespace Explicit.Validation.FluentValidation;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>, IValidate<TValue>
    where TValue : notnull
{
    internal FluentValidator()
    {
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<TValue> context)
    {
        
    }
}

public static class FluentValidator
{
    public static OneOf<Success, ValidationErrors> Validate<T>(
        T validatable,
        Action<FluentValidator<T>> validationSetup)
        where T : IValidatable
    {
        var fluentValidator = new FluentValidator<T>(validatable);
        validationSetup(fluentValidator);

        return fluentValidator.Validate(validatable).ToResult();
    }
    
    public static void ValidateCollection<TFrom, TMethod, TValue>(
        IReadOnlyCollection<TValue> collection,
        Validator<TFrom> context)
        where TMethod : IValidate<TValue>
    {
        collection.ForEach((property, propertyIndex) =>
        {
            var errors = TMethod.Validate(property).Match<IReadOnlyCollection<string>>(
                success => Array.Empty<string>(),
                errors => errors.ErrorMessages);

            errors.ForEach(error => context.AddFailure(new ValidationFailure($"[{propertyIndex}]", error)));
        });
    }
}