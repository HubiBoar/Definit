using Explicit.Validation.NewFluent.Primitives;

namespace Explicit.Validation.NewFluent;

public static class IsValidExtensions
{
    public static IsValid<TValue> IsValid<TValue>(this TValue value)
        where TValue : IValidate<TValue>
    {
        return NewFluent.IsValid<TValue>.Create(value);
    }
    
    public static IsValid<Value<TValue, TMethod>> IsValid<TValue, TMethod>(this TValue value)
        where TMethod : IValidationRule<TValue>
        where TValue : notnull
    {
        return NewFluent.IsValid<Value<TValue, TMethod>>.Create(new Value<TValue, TMethod>(value));
    }
    
    public static IReadOnlyCollection<IsValid<TValue>> IsValid<TValue>(this IEnumerable<TValue> values)
        where TValue : IValidate<TValue>
    {
        return values.Select(IsValid).ToArray();
    }
}