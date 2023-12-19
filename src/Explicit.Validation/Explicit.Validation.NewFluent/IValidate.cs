using Explicit.Validation.NewFluent.Fluent;

namespace Explicit.Validation.NewFluent;

public interface IValidate<TValue>
    where TValue : IValidate<TValue>
{
    public static abstract void SetupValidation(FluentValidator<TValue> validator);
}

public interface IValidationRule<in TValue>
    where TValue : notnull
{
    public static abstract void SetupRule<TFrom>(IRuleBuilder<TFrom, TValue> ruleBuilder);
}