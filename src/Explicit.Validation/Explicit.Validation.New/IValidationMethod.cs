using OneOf;
using OneOf.Types;

namespace Explicit.Validation.New;

public sealed class IsValid<TValue>
{
    public OneOf<Valid<TValue>, ValidationErrors> Result { get; }

    internal IsValid(OneOf<TValue, ValidationErrors> result)
    {
        Result = result.Match<OneOf<Valid<TValue>, ValidationErrors>>(v => new Valid<TValue>(v), e => e);
    }
}

public sealed class ValidationErrors
{
}

public sealed class Valid<TValue>
{
    public TValue ValidValue { get; }

    internal Valid(TValue validValue)
    {
        ValidValue = validValue;
    }
}

public interface IValidationExtension<TValue>
{
    OneOf<Success, ValidationErrors> Validate(TValue value);
}

internal class ValidationMethodExtension<TValue, TMethod> : IValidationExtension<TValue>
    where TMethod : IValidationMethod<TValue>
    where TValue : notnull
{
    public OneOf<Success, ValidationErrors> Validate(TValue value)
    {
        TMethod.SetupValidation(value);
    }
}

public class ValidationContext<TValue>
    where TValue : notnull
{
    private TValue Value { get; }
    private List<IValidationExtension<TValue>> _extensions { get; }

    private ValidationContext(TValue value)
    {
        Value = value;
        _extensions = new List<IValidationExtension<TValue>>();
    }

    public void AddExtension(IValidationExtension<TValue> extension)
    {
        _extensions.Add(extension);
    }

    public void AddMethod<TMethod>()
        where TMethod : IValidationMethod<TValue>
    {
        _extensions.Add(new ValidationMethodExtension<TValue,TMethod>());
    }
    
    public static IsValid<TValue> IsValid(TValue value)
    {
        var context = new ValidationContext<TValue>(value);

        TMethod.SetupValidation(context);
        
        return new IsValid<TValue>(value, );
    }
}

public interface IValidationMethod<TValue>
    where TValue : notnull
{
    public static abstract void SetupValidation(ValidationContext<TValue> context);
}