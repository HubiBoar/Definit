using Explicit.Validation;
using Newtonsoft.Json;

namespace Explicit.Primitives;

public interface IJsonStaticConvertable
{
    static abstract string ToJson(object obj);
    
    static abstract object FromJson(string json);

    static abstract bool CanConvert(Type type);

    internal static string ToJsonInternal(object obj)
    {
        return (string)obj.GetType().GetMethod(nameof(IJsonStaticConvertable.ToJson))!.Invoke(null, new[] {obj})!;
    }
    
    internal static object FromJsonInternal(Type objectType, string json)
    {
        return objectType.GetMethod(nameof(IJsonStaticConvertable.FromJson))!.Invoke(null, new object?[] {json})!;
    }
    
    internal static bool CanConvertInternal(Type objectType)
    {
        if (objectType.IsAssignableTo(typeof(IJsonStaticConvertable)))
        {
            return (bool)objectType.GetMethod(nameof(IJsonStaticConvertable.CanConvert))!.Invoke(null, new object?[] {objectType}) !;
        }

        return false;
    }
}

[Newtonsoft.Json.JsonConverter(typeof(NewtonsoftStaticConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(SystemJsonStaticConverter))]
public sealed class Value<TValue, TMethod> : IValidatable, IJsonStaticConvertable
    where TMethod : IValidationMethod<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue() => _value;

    public override string? ToString() => JsonConvert.SerializeObject(_value);

    public OneOf<Success, ValidationErrors> Validate()
    {
        return TMethod.Validate(_value);
    }

    public static implicit operator Value<TValue, TMethod>(TValue value)
    {
        return new Value<TValue, TMethod>(value);
    }

    public static implicit operator TValue(Value<TValue, TMethod> self)
    {
        return self._value;
    }

    public static object FromJson(string json)
    {
        var value = JsonConvert.DeserializeObject<TValue>(json)!;

        return new Value<TValue, TMethod>(value);
    }

    public static string ToJson(object obj)
    {
        var value = obj as Value<TValue, TMethod>;

        return JsonConvert.SerializeObject(value!._value);
    }

    public static bool CanConvert(Type type)
    {
        return type == typeof(Value<TValue, TMethod>);
    }
}