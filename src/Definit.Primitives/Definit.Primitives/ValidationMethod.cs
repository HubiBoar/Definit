using Definit.Json;
using Definit.Validation;
using Newtonsoft.Json;

namespace Definit.Primitives;

public abstract record ValidationMethod<TSelf, TValue> : IValidate<TValue>
    where TSelf : ValidationMethod<TSelf, TValue>, IValidate<TValue>, new()
    where TValue : notnull
{
    public static ValidationResult Validate(Validator<TValue> context) => new TSelf().Validation(context);

    protected abstract ValidationResult Validation(Validator<TValue> context);

    public static IsValid<Value> Validate(TValue value) => new Value(value).IsValid();
   
    [SystemJsonStaticConverter]
    public sealed class Value : IValidate<Value>, IJsonStaticConvertable<Value>,  IValue<TValue>
    {
        private readonly TValue _value;

        public Value(TValue value)
        {
            _value = value;
        }

        public TValue GetValue() => _value;
    
        public static ValidationResult Validate(Validator<Value> context)
        {
            return TSelf.Validate(new Validator<TValue>(context.Value));
        }

        public static implicit operator Value(TValue value) => new (value);

        public static implicit operator TValue(Value self)  => self._value;

        public static string ToJson(Value value) => JsonConvert.SerializeObject(value!._value);
        public static bool CanConvert(Type type) => type == typeof(Value);
        public override string? ToString()       => JsonConvert.SerializeObject(_value);

        public static Value FromJson(string json)
        {
            var value = JsonConvert.DeserializeObject<TValue>(json)!;

            return new Value(value);
        }
    } 
}