using Definit.Primitives;
using MoreLinq.Extensions;

namespace Definit.Validation.FluentValidation;

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
        return builder.Custom((property, context) =>
        {
            if(property.IsValid<TValue, TMethod>().Is(out ValidationErrors errors))
            {
                context.AddFailure(errors.Description);
            }
        });
    }

    public static void ValidateCollection<TMethod, TValue>(
        this ValidationContext<TValue> context,
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        context.ValidateCollection<TValue, TMethod, TValue>(collection);
    }

    public static void ValidateCollection<TFrom, TMethod, TValue>(
        this ValidationContext<TFrom> context,
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        collection.ForEach((property, propertyIndex) =>
        {
            if(property.IsValid<TValue, TMethod>().Is(out ValidationErrors errors))
            {                    
                foreach(var error in errors.Errors)
                {
                    foreach(var value in error.Value)
                    {
                        context.AddFailure(context.DisplayName, $"(Index:{propertyIndex}) {value}");
                    }
                }
            }
        });
    }
}