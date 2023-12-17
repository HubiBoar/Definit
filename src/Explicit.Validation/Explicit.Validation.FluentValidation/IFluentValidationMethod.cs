namespace Explicit.Validation.FluentValidation;

public interface IFluentValidationRule<TValue>
{
    public static abstract void SetupValidation<TFrom>(RuleBuilder<TFrom, TValue> ruleBuilder);
}

public interface IFluentValidationMethod<TRule, in TValue> : IValidationMethod<TValue>
    where TRule : IFluentValidationRule<TValue>
    where TValue : notnull
{
    static OneOf<Success, ValidationErrors> IValidationMethod<TValue>.Validate(TValue value)
    {
        var fluentValidator = new FluentValidator<TValue>();

        var rule = fluentValidator.RuleFor(from => value);

        TRule.SetupValidation(new RuleBuilder<TValue, TValue>(rule));

        rule.Must(x => true).WithName("Value");

        return fluentValidator.Validate(value).ToResult();
    }
}

public interface IFluentValidationRuleMethod<TSelf, TValue> : 
    IFluentValidationMethod<TSelf, TValue>,
    IFluentValidationRule<TValue>
    where TSelf : IFluentValidationRuleMethod<TSelf, TValue>
    where TValue : notnull
{
}