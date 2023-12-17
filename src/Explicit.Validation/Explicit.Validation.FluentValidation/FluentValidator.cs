using Explicit.Validation;
using MoreLinq.Extensions;

namespace Explicit.Validation.FluentValidation;

public sealed class FluentValidator<T> : AbstractValidator<T>
    where T : notnull
{
    internal FluentValidator(T validatable)
    {
        var validatableType = validatable.GetType();
        
        var validatableProperties = validatableType
            .GetProperties()
            .Where(x => x.PropertyType.IsAssignableTo(typeof(IValidatable)))
            .ToArray();
    
        foreach (var property in validatableProperties)
        {
            RuleFor(x => (IValidatable)property.GetValue(x)!).Custom((v, context) =>
            {
                var errors = v.IsValid().Match<IReadOnlyCollection<string>>(
                    _ => Array.Empty<string>(),
                    errors => errors.ValidationErrors.ErrorMessages);

                foreach (var error in errors)
                {
                    context.AddFailure(new ValidationFailure(property.Name, error));
                }
            });
        }
    }

    internal FluentValidator()
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
        ValidationContext<TFrom> context)
        where TMethod : IValidationMethod<TValue>
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