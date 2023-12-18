namespace Explicit.Validation;

public static class IsValidExtensions
{
    public static IsValid<TValue> IsValid<TValue>(this TValue value)
        where TValue : IValidate<TValue>
    {
        return Validation.IsValid<TValue>.Create(value);
    }
    
    public static IReadOnlyCollection<IsValid<TValue>> IsValid<TValue>(this IEnumerable<TValue> values)
        where TValue : IValidate<TValue>
    {
        return values.Select(IsValid).ToArray();
    }
    
    public static OneOf<Success, ValidationErrors> Validate<TValue>(this TValue value)
        where TValue : IValidate<TValue>
    {
        return Validator<TValue>.Validate<TValue>(value);
    }
}