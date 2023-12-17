namespace Explicit.Validation.FluentValidation;

public interface IFluentValidatable<TSelf> : IFluentValidatableSetup<TSelf>, IValidatable, IValidate<TSelf>
    where TSelf : IFluentValidatable<TSelf>
{
    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return FluentValidator.Validate((TSelf)this, TSelf.SetupValidation);
    }

    static OneOf<Success, ValidationErrors> IValidate<TSelf>.Validate(TSelf value)
    {
        return FluentValidator.Validate(value, TSelf.SetupValidation);
    }
}

public interface IFluentValidatableSetup<TFrom>
    where TFrom : notnull
{
    public static abstract void SetupValidation(FluentValidator<TFrom> validator);
}

public static class FluentValidateExtensions
{
    public static FluentValidator<TValue> Fluent<TValue>(this Validator<TValue> validator)
        where TValue : notnull
    {
        return new FluentValidator<TValue>();
    }
}