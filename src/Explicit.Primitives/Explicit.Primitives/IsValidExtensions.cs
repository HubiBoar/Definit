using Explicit.Validation;

namespace Explicit.Primitives;

public static class IsValidExtensions
{
    public static IsValid<Value<TValue, TMethod>> IsValid<TValue, TMethod>(this TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        return Validation.IsValid<Value<TValue, TMethod>>.Create(new Value<TValue, TMethod>(value));
    }
}