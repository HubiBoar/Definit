namespace Explicit.Validation.FluentValidation;

public static class ValidatableExtensions
{
    public static IRuleBuilderOptionsConditions<T, TValue> ValidateSelf<T, TValue>(
        this IRuleBuilder<T, TValue> builder)
        where TValue : IValidate<TValue>
    {
        return builder.ValidateSelf<T, TValue, TValue>();
    }
    
    public static IRuleBuilderOptionsConditions<T, TValue> ValidateSelf<T, TValue>(
        this IRuleBuilderOptions<T, TValue> builder)
        where TValue : IValidate<TValue>
    {
        return builder.ValidateSelf<T, TValue, TValue>();
    }

    public static IRuleBuilderOptionsConditions<T, TValue> ValidateSelf<T, TValue, TMethod>(
        this IRuleBuilder<T, TValue> builder)
        where TValue : notnull
        where TMethod : IValidate<TValue>
    {
        return builder.Custom((validatable, context) =>
        {
            var errors = Validator<TValue>.Validate<TMethod>(validatable).Match<IReadOnlyCollection<string>>(
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