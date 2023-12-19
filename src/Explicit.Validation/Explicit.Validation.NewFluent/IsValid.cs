using Explicit.Validation.NewFluent.Fluent;

namespace Explicit.Validation.NewFluent;

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

    internal static IsValid<TValue> Create(TValue value)
    {
        var validator = new FluentValidator<TValue>();
        var result = validator.Validate(value);
        
        if (result.IsValid)
        {
            return new IsValid<TValue>(new Valid<TValue>(value));
        }

        var errors = new ValidationErrors(result.Errors.Select(x => x.ErrorMessage).ToArray());

        return new IsValid<TValue>(errors);
    }
}

public sealed class Valid<TValue>
    where TValue : IValidate<TValue>
{
    public TValue ValidValue { get; }

    internal Valid(TValue validValue)
    {
        ValidValue = validValue;
    }
}
