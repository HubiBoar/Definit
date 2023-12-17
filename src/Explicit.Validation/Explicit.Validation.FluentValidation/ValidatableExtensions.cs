using Explicit.Validation;

namespace Explicit.Validation.FluentValidation;

public static class ValidatableExtensions
{
    public static IRuleBuilderOptionsConditions<T, TProperty> ValidateSelf<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> builder)
        where TProperty : IValidate<TProperty>
    {
        return builder.Custom((validatable, context) =>
        {
            var errors = validatable.IsValid().Match<IReadOnlyCollection<string>>(
                _ => Array.Empty<string>(),
                errors => errors.ErrorMessages);
            
            foreach (var error in errors)
            {
                context.AddFailure(error);
            }
        });
    }

    public static IRuleBuilderOptionsConditions<T, TProperty> ValidateSelf<T, TProperty>(
        this IRuleBuilder<T, TProperty> builder)
        where TProperty : IValidatable
    {
        return builder.Custom((validatable, context) =>
        {
            var errors = validatable.IsValid().Match<IReadOnlyCollection<string>>(
                _ => Array.Empty<string>(),
                errors => errors.ErrorMessages);
            
            foreach (var error in errors)
            {
                context.AddFailure(error);
            }
        });
    }

    internal static OneOf<Success, ValidationErrors> ToResult(this ValidationResult result)
    {
        if (result.IsValid)
        {
            return new Success();
        }

        var errors = new ValidationErrors(result.Errors.Select(x => x.ErrorMessage).ToArray());

        return errors;
    }
}