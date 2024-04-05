using Definit.Utils;
using Definit.Results;
using OneOf.Else;
using Error = Definit.Results.Error;

namespace Definit.Validation;

public sealed class IsValid<TValue> : ResultBase<Valid<TValue>, ValidationErrors>
    where TValue : IValidate<TValue>
{
    public Result<TValue, ValidationErrors> Basic { get; }
    public ValidationResult Success { get; }

    private IsValid(OneOf<Valid<TValue>, ValidationErrors, Error> result) : base(result)
    {
        Basic   = result.Match<Result<TValue, ValidationErrors>>(x => x.ValidValue, e => e, e => e);
        Success = result.Match(x => ValidationResult.Success, e => e, e => e);
    }

    public OneOfElse<OneOf<ValidationErrors, Error>> Is(out TValue value)
    {
        var result = Is(out Valid<TValue> valid);

        value = default!;
        if(result)
        {
            value = valid.ValidValue;
        }

        return result;
    }

    public IsValid(Valid<TValue> value) : this((OneOf<Valid<TValue>, ValidationErrors, Error>)value) {}
    public IsValid(Error error)         : this((OneOf<Valid<TValue>, ValidationErrors, Error>)error) {}

    public static implicit operator IsValid<TValue> (Error value)            => new (value);
    public static implicit operator IsValid<TValue> (ValidationErrors value) => new (value);

    public static IsValid<TValue> Null()
    {
        return new IsValid<TValue>(ValidationErrors.Null(DefinitType.GetTypeVerboseName<TValue>()));
    }

    public static IsValid<TValue> Create(TValue? value)
    {
        if (value is null)
        {
            return Null();
        }
        
        var context = new Validator<TValue>(value);
        return TValue.Validate(context).Match(
            success => new IsValid<TValue>(new Valid<TValue>(value)),
            validationError => new IsValid<TValue>(validationError),
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