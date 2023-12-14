using Explicit.Validation;

namespace Explicit.Validation.FluentValidation;

public interface IFluentValidatable<TSelf> : IFluentValidatableSetup<TSelf>, IValidatable, IValidationMethod<TSelf>
    where TSelf : IFluentValidatable<TSelf>
{
    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return FluentValidator.Validate((TSelf)this, TSelf.SetupValidation);
    }

    static OneOf<Success, ValidationErrors> IValidationMethod<TSelf>.Validate(TSelf value)
    {
        return FluentValidator.Validate(value, TSelf.SetupValidation);
    }
}

public interface IFluentValidatableSetup<TFrom>
    where TFrom : notnull
{
    public static abstract void SetupValidation(FluentValidator<TFrom> validator);
}