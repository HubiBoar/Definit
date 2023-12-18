namespace Explicit.Validation.FluentValidation;

public static class FluentValidateExtensions
{
    public static OneOf<Success, ValidationErrors> Fluent<TValue>(this Validator<TValue> context, Action<FluentValidator<TValue>> validator)
        where TValue : IValidate<TValue>
    {
        var fluentValidator = new FluentValidator<TValue>();

        validator(fluentValidator);
        
        return fluentValidator.Validate(context.Value).ToResult();
    }
    
    public static OneOf<Success, ValidationErrors> FluentRule<TValue>(this Validator<TValue> context, Action<RuleBuilder<TValue, TValue>> validator)
        where TValue : notnull
    {
        var fluentValidator = new FluentValidator<TValue>();

        var rule = fluentValidator.RuleFor(from => context.Value);

        validator(new RuleBuilder<TValue, TValue>(rule));

        rule.Must(x => true).WithName("Value");

        return fluentValidator.Validate(context.Value).ToResult();
    }
}