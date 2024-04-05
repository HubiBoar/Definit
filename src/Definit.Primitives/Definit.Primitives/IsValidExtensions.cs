using Definit.Validation;
using OneOf.Else;
using Error = Definit.Results.Error;

namespace Definit.Primitives;

public static class IsValidExtensions
{
    public static IsValid<Value<TValue, TMethod>> IsValid<TValue, TMethod>(this TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        return IsValid<Value<TValue, TMethod>>.Create(new Value<TValue, TMethod>(value));
    }

    public static OneOfElse<OneOf<ValidationErrors, Error>> Is<TValue, TMethod>(this Value<TValue, TMethod> oneOf, out TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        return oneOf.IsValid().Is(out value);
    }

    public static OneOfElse<OneOf<ValidationErrors, Error>> Is<TValue, TMethod>(this IsValid<Value<TValue, TMethod>> oneOf, out TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        var result = oneOf.Is(out Valid<Value<TValue, TMethod>> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }

    public static OneOfElse<ValidationErrors> Is<TValue, TMethod>(this OneOf<Valid<Value<TValue, TMethod>>, ValidationErrors> oneOf, out TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        var result = oneOf.Is(out Valid<Value<TValue, TMethod>> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }

    public static OneOfElse<Error> Is<TValue, TMethod>(this OneOf<Valid<Value<TValue, TMethod>>, Error> oneOf, out TValue value)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        var result = oneOf.Is(out Valid<Value<TValue, TMethod>> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }
        
        return result;
    }
}