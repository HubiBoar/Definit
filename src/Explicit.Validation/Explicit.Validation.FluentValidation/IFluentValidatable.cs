namespace Explicit.Validation.FluentValidation;

public static class FluentValidateExtensions
{
    public static OneOf<Success, ValidationErrors> Fluent<TValue>(this Validator<TValue> context, Action<FluentValidator<TValue>> validator)
        where TValue : notnull
    {
        return FluentValidator<TValue>.Validate(context.Value, validator);
    }
    
    public static OneOf<Success, ValidationErrors> FluentRule<TValue>(this Validator<TValue> context, Action<IRuleBuilder<TValue, TValue>> ruleBuilder)
        where TValue : notnull
    {
        return FluentValidator<TValue>.Validate(context.Value, validator =>
        {
            var rule = validator.RuleFor(from => context.Value);

            ruleBuilder(rule);
        });
    }
}