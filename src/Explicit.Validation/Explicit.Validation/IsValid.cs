namespace Explicit.Validation;

public sealed class IsValid<TValue> : OneOfBase<Valid<TValue>, ValidationErrors>
    where TValue : IValidate<TValue>
{
    public OneOf<TValue, ValidationErrors> Basic { get; }

    private IsValid(ValidationErrors input) : base(input)
    {
        Basic = input;
    }
    
    private IsValid(Valid<TValue> input) : base(input)
    {
        Basic = input.ValidValue;
    }

    public static IsValid<TValue> Create(TValue value)
    {
        var context = new Validator<TValue>(value);
        return TValue.Validate(context).Match<IsValid<TValue>>(
            success => new IsValid<TValue>(new Valid<TValue>(value)),
            error => new IsValid<TValue>(error));
    }
}

public class Valid<TValue>
    where TValue : IValidate<TValue>
{
    public TValue ValidValue { get; }

    internal Valid(TValue value)
    {
        ValidValue = value;
    }
}