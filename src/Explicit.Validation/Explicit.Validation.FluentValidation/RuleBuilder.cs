using FluentValidation.Validators;

namespace Explicit.Validation.FluentValidation;

public class RuleBuilder<TFrom, TValue> : IRuleBuilder<TFrom, TValue>
{
    private IRuleBuilder<TFrom, TValue> Rule { get; }

    public RuleBuilder(IRuleBuilder<TFrom, TValue> rule)
    {
        Rule = rule;
    }

    public IRuleBuilderOptions<TFrom, TValue> SetValidator(
        IPropertyValidator<TFrom, TValue> validator)
    {
        return Rule.SetValidator(validator);
    }

    public IRuleBuilderOptions<TFrom, TValue> SetAsyncValidator(
        IAsyncPropertyValidator<TFrom, TValue> validator)
    {
        return Rule.SetAsyncValidator(validator);
    }

    public IRuleBuilderOptions<TFrom, TValue> SetValidator(
        IValidator<TValue> validator,
        params string[] ruleSets)
    {
        return Rule.SetValidator(validator, ruleSets);
    }

    public IRuleBuilderOptions<TFrom, TValue> SetValidator<TValidator>(
        Func<TFrom, TValidator> validatorProvider,
        params string[] ruleSets)
        where TValidator : IValidator<TValue>
    {
        return Rule.SetValidator(validatorProvider, ruleSets);
    }

    public IRuleBuilderOptions<TFrom, TValue> SetValidator<TValidator>(
        Func<TFrom, TValue, TValidator> validatorProvider,
        params string[] ruleSets)
        where TValidator : IValidator<TValue>
    {
        return Rule.SetValidator(validatorProvider, ruleSets);
    }
}