using Explicit.Primitives;
using MoreLinq.Extensions;

namespace Explicit.Validation.FluentValidation;

public static class ValidateExtensions
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
            var errors = validatable.IsValid<TValue, TMethod>().Match<IReadOnlyCollection<string>>(
                _ => Array.Empty<string>(),
                errors => errors.ErrorMessages);
            
            errors.ForEach(context.AddFailure);
        });
    }

    public static void Use<TMethod, TValue>(
        this ValidationContext<TValue> context,
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        context.Use<TValue, TMethod, TValue>(collection);
    }

    public static void Use<TFrom, TMethod, TValue>(
        this ValidationContext<TFrom> context,
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidate<TValue>
        where TValue : notnull
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