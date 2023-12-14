using Explicit.Validation;

namespace Explicit.Primitives;

public interface IValue<out TValue, TMethod> : IValidatable
    where TMethod : IValidationMethod<TValue>
{
    public TValue GetValue();

    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return TMethod.Validate(GetValue());
    }
}

public class Value<TValue, TMethod> : IValue<TValue, TMethod>
    where TMethod : IValidationMethod<TValue>
{
    private readonly TValue _value;

    public Value(TValue value)
    {
        _value = value;
    }

    public TValue GetValue() => _value;

    public override string? ToString() => GetValue()?.ToString();
    
    public static implicit operator Value<TValue, TMethod>(TValue value)
    {
        return new Value<TValue, TMethod>(value);
    }

    public static implicit operator TValue(Value<TValue, TMethod> self)
    {
        return self._value;
    }
}

public class Value<TMethod> : IValue<string, TMethod>
    where TMethod : IValidationMethod<string>
{
    private readonly string _value;

    private Value(string value)
    {
        _value = value;
    }

    public string GetValue() => _value;

    public override string ToString() => GetValue();

    public static implicit operator Value<TMethod>(string value)
    {
        return new Value<TMethod>(value);
    }

    public static implicit operator string(Value<TMethod> self)
    {
        return self._value;
    }
}

public record ValueRecord<TMethod>(string Value) : IValue<string, TMethod>
    where TMethod : IValidationMethod<string>
{
    public string GetValue() => Value;
    
    public override string ToString() => GetValue();

    public static implicit operator ValueRecord<TMethod>(string value)
    {
        return new ValueRecord<TMethod>(value);
    }

    public static implicit operator string(ValueRecord<TMethod> self)
    {
        return self.Value;
    }
}

public record ValueRecord<TValue, TMethod>(TValue Value) : IValue<TValue, TMethod>
    where TMethod : IValidationMethod<TValue>
{
    public TValue GetValue() => Value;

    public override string? ToString() => GetValue()?.ToString();

    public static implicit operator ValueRecord<TValue, TMethod>(TValue value)
    {
        return new ValueRecord<TValue, TMethod>(value);
    }

    public static implicit operator TValue(ValueRecord<TValue, TMethod> self)
    {
        return self.Value;
    }
}