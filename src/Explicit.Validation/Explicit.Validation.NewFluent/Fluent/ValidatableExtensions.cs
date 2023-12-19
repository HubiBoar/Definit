using Explicit.Validation.NewFluent.Primitives;
using MoreLinq.Extensions;

namespace Explicit.Validation.NewFluent.Fluent;

public static class ValidateExtensions
{
    public static IRuleBuilderOptionsConditions<T, TValue> ValidateSelf<T, TValue>(
        this IRuleBuilder<T, TValue> builder)
        where TValue : IValidate<TValue>
    {
        return builder.Custom((value, context) =>
        {
            var errors = value.IsValid().Match<IReadOnlyCollection<string>>(
                _ => Array.Empty<string>(),
                errors => errors.ErrorMessages);
            
            foreach (var error in errors)
            {
                context.AddFailure(error);
            }
        });
    }
    
    public static void Use<TFrom, TMethod, TValue>(
        this ValidationContext<TFrom> context,
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidationRule<TValue> where TValue : notnull
    {
        collection.ForEach((property, propertyIndex) =>
        {
            var errors = property.IsValid<TValue, TMethod>().Match<IReadOnlyCollection<string>>(
                success => Array.Empty<string>(),
                errors => errors.ErrorMessages);

            errors.ForEach(error => context.AddFailure(new ValidationFailure($"[{propertyIndex}]", error)));
        });
    }
}