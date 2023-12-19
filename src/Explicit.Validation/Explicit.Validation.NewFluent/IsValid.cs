using OneOf;

namespace Explicit.Validation.NewFluent;

public sealed class IsValid<TValue>
    where TValue : IValidate<TValue>
{
    public OneOf<Valid<TValue>, ValidationErrors> Result { get; }

    internal IsValid(OneOf<Valid<TValue>, ValidationErrors> result)
    {
        Result = result;
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
