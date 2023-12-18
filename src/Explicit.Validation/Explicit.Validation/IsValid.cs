namespace Explicit.Validation;

public sealed record Valid;

public sealed class Valid<TValue>
    where TValue : IValidate<TValue>
{
    public TValue ValidValue { get; }

    internal Valid(TValue validValue)
    {
        ValidValue = validValue;
    }

    public Valid<TParam> GetParameter<TParam>(Func<TValue, TParam> getParameter)
        where TParam : IValidate<TParam>
    {
        return new Valid<TParam>(getParameter(ValidValue));
    }

    public static explicit operator TValue(Valid<TValue> valid)
    {
        return valid.ValidValue;
    }

    public static explicit operator Valid(Valid<TValue> valid)
    {
        return new Valid();
    }
}

public sealed class IsValid<TValue> : OneOfBase<Valid<TValue>, ValidationErrors>
    where TValue : IValidate<TValue>
{
    private IsValid(Valid<TValue> value)
        : base(value)
    {
    }

    private IsValid(ValidationErrors validationErrors)
        : base(validationErrors)
    {
    }

    public static IsValid<TValue> Create(TValue validatable)
    {
        var validationResult = validatable.Validate();

        return validationResult.Match<IsValid<TValue>>(
            success => new IsValid<TValue>(new Valid<TValue>(validatable)),
            errors => new IsValid<TValue>(errors));
    }
}