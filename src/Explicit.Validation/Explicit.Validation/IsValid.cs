namespace Explicit.Validation;

public sealed record Valid;

public sealed class Valid<T>
    where T : IValidatable
{
    public T ValidValue { get; }

    internal Valid(T validValue)
    {
        ValidValue = validValue;
    }

    public Valid<TParam> GetParameter<TParam>(Func<T, TParam> getParameter)
        where TParam : IValidatable
    {
        return new Valid<TParam>(getParameter(ValidValue));
    }

    public static explicit operator T(Valid<T> valid)
    {
        return valid.ValidValue;
    }

    public static explicit operator Valid(Valid<T> valid)
    {
        return new Valid();
    }
}

public sealed class InValid<T>
    where T : IValidatable
{
    public ValidationErrors ValidationErrors { get; }

    public string Message => ValidationErrors.Message;
    
    public T InValidValue { get; }

    internal InValid(ValidationErrors validationErrors, T inValidValue)
    {
        ValidationErrors = validationErrors;
        InValidValue = inValidValue;
    }

    public static explicit operator ValidationErrors(InValid<T> valid)
    {
        return valid.ValidationErrors;
    }
}

public sealed class IsValid<T> : OneOfBase<Valid<T>, InValid<T>>
    where T : IValidatable
{
    public OneOf<T, ValidationErrors> Basic => Match<OneOf<T, ValidationErrors>>(v => v.ValidValue, e => e.ValidationErrors);
    
    private IsValid(Valid<T> value)
        : base(value)
    {
    }

    private IsValid(InValid<T> inValid)
        : base(inValid)
    {
    }

    public static IsValid<T> Create(T validatable)
    {
        var validationResult = validatable.Validate();

        return validationResult.Match<IsValid<T>>(
            success => new IsValid<T>(new Valid<T>(validatable)),
            errors => new IsValid<T>(new InValid<T>(errors, validatable)));
    }
}