using Definit.Utils;
using Definit.Results;

namespace Definit.Validation;

public sealed class IsValid<TValue> : Result<Valid<TValue>>
    where TValue : IValidate<TValue>
{
    public Result<TValue> Basic { get; }
    public Result Success { get; }

    private IsValid(Error input) : base(input)
    {
        Basic = input;
        Success = input;
    }
    
    private IsValid(Valid<TValue> input) : base(input)
    {
        Basic = input.ValidValue;
        Success = Result.Success;
    }

    public static IsValid<TValue> Error(Error errors)
    {
        return new IsValid<TValue>(errors);
    }

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