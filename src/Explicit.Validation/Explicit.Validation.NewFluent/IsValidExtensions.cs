using Explicit.Validation.NewFluent.Fluent;

namespace Explicit.Validation.NewFluent;

public static class IsValidExtensions
{
    public static IsValid<TValue> IsValid<TValue>(this TValue value)
        where TValue : IValidate<TValue>
    {
        return FluentValidator<TValue>.IsValid(value);
    }
    
    public static IReadOnlyCollection<IsValid<TValue>> IsValid<TValue>(this IEnumerable<TValue> values)
        where TValue : IValidate<TValue>
    {
        return values.Select(IsValid).ToArray();
    }
}