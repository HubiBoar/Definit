namespace Explicit.Validation;

public sealed class Validator<TValue>
    where TValue : IValidate<TValue>
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
}

public interface IValidate<TSelf>
    where TSelf : IValidate<TSelf>
{
    public static abstract OneOf<Success, ValidationErrors> Validate(Validator<TSelf> context);
}