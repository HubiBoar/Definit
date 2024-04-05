using OneOf.Else;
using Definit.Results;

namespace Definit.Validation;

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

    public static OneOfElse<ValidationErrors> Is<TValue>(this OneOf<Valid<TValue>, ValidationErrors> oneOf, out TValue value)
        where TValue : IValidate<TValue>
    {
        var result = oneOf.Is(out Valid<TValue> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }

    public static OneOfElse<Error> Is<TValue>(this OneOf<Valid<TValue>, Error> oneOf, out TValue value)
        where TValue : IValidate<TValue>
    {
        var result = oneOf.Is(out Valid<TValue> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }

    public static bool Else<TValue>(this OneOfElse<Valid<TValue>> oneOf, out TValue value)
        where TValue : IValidate<TValue>
    {
        var result = oneOf.Else(out Valid<TValue> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }
}