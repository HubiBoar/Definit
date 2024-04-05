using Definit.Json;
using Definit.Validation;
using Newtonsoft.Json;

namespace Definit.Primitives;

[SystemJsonStaticConverter]
public sealed class Value<TValue, TMethod> : IValidate<Value<TValue, TMethod>>, IJsonStaticConvertable<Value<TValue, TMethod>>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue() => _value;

    public static ValidationResult Validate(Validator<Value<TValue, TMethod>> context)
    {
        return TMethod.Validate(new Validator<TValue>(context.Value._value));
    }

    public static implicit operator Value<TValue, TMethod>(TValue value) => new (value);

    public static implicit operator TValue(Value<TValue, TMethod> self)  => self._value;

    public static string ToJson(Value<TValue, TMethod> value) => JsonConvert.SerializeObject(value!._value);
    public override string? ToString()                        => JsonConvert.SerializeObject(_value);
    public static bool CanConvert(Type type)                  => type == typeof(Value<TValue, TMethod>);

    public static Value<TValue, TMethod> FromJson(string json)
    {
        var value = JsonConvert.DeserializeObject<TValue>(json)!;

        return new Value<TValue, TMethod>(value);
    }
}