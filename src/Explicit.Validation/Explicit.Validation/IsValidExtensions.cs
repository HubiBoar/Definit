namespace Explicit.Validation;

public static class IsValidExtensions
{
    public static IsValid<TValue> IsValid<TValue>(this TValue value)
        where TValue : IValidate<TValue>
    {
        return Validator<TValue>.IsValid<TValue>(value);
    }
    
    public static IReadOnlyCollection<IsValid<TValue>> IsValid<TValue>(this IEnumerable<TValue> values)
        where TValue : IValidate<TValue>
    {
        return Validator<TValue>.IsValid<TValue>(values);
    }
}