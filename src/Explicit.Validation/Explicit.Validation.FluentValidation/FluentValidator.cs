using MoreLinq.Extensions;

namespace Explicit.Validation.FluentValidation;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>
    where TValue : notnull
{
}

public static class FluentValidator
{
    public static void ValidateCollection<TFrom, TMethod, TValue>(
        IReadOnlyCollection<TValue> collection,
        ValidationContext<TFrom> context)
        where TMethod : IValidate<TValue> where TValue : notnull
    {
        collection.ForEach((property, propertyIndex) =>
        {
            var errors = Validator<TValue>.Validate<TMethod>(property).Match<IReadOnlyCollection<string>>(
                success => Array.Empty<string>(),
                errors => errors.ErrorMessages);

            errors.ForEach(error => context.AddFailure(new ValidationFailure($"[{propertyIndex}]", error)));
        });
    }
}