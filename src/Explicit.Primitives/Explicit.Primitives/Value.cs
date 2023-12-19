using Explicit.Utils.Json;
using Explicit.Validation;
using Newtonsoft.Json;

namespace Explicit.Primitives;

[SystemJsonStaticConverter]
public sealed class Value<TValue, TMethod> : IValidate<Value<TValue, TMethod>>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue()
    {
        return _value;
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<Value<TValue, TMethod>> context)
    {
        return TMethod.Validate(new Validator<TValue>(context.Value._value));
    }

    public override string? ToString() => JsonConvert.SerializeObject(_value);

    public static implicit operator Value<TValue, TMethod>(TValue value)
    {
        return new Value<TValue, TMethod>(value);
    }

    public static implicit operator TValue(Value<TValue, TMethod> self)
    {
        return self._value;
    }

    public static string ToJson(Value<TValue, TMethod> value)
    {
        return JsonConvert.SerializeObject(value!._value);
    }

    public static Value<TValue, TMethod> FromJson(string json)
    {
        var value = JsonConvert.DeserializeObject<TValue>(json)!;

        return new Value<TValue, TMethod>(value);
    }

    public static bool CanConvert(Type type)
    {
        return type == typeof(Value<TValue, TMethod>);
    }
}
