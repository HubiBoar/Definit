namespace Explicit.Validation;

public sealed class Validator<TValue>
    where TValue : notnull
{
    public TValue Value { get; }

    internal Validator(TValue value)
    {
        Value = value;
    }

    public static OneOf<Success, ValidationErrors> Validate<TMethod>(TValue value)
        where TMethod : IValidate<TValue>
    {
        var context = new Validator<TValue>(value);
        return TMethod.Validate(context);
    }
    
    public static IsValid<TValue> IsValid<TMethod>(TValue value)
        where TMethod : IValidate<TValue>
    {
        return Validation.IsValid<TValue>.Create(value, r => TMethod.Validate(new Validator<TValue>(r)));
    }

    public static IReadOnlyCollection<IsValid<TValue>> IsValid<TMethod>(IEnumerable<TValue> values)
        where TMethod : IValidate<TValue>
    {
        return values.Select(IsValid<TMethod>).ToArray();
    }
}

public interface IValidate<TValue>
    where TValue : notnull
{
    public static abstract OneOf<Success, ValidationErrors> Validate(Validator<TValue> context);
}