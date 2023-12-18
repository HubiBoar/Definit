using System.Text.Json;

namespace Explicit.Validation.FluentValidation;

public interface IFluentValidationRule<TMethod, TValue>
    where TMethod : IFluentValidationRule<TMethod, TValue>
    where TValue : notnull
{
    static OneOf<Success, ValidationErrors> IValidate<TValue>.Validate(Validator<TValue> context)
    {
        var fluentValidator = new FluentValidator<TValue>();

        var rule = fluentValidator.RuleFor(from => context.Value);

        TMethod.SetupValidation(new RuleBuilder<TValue, TValue>(rule));

        rule.Must(x => true).WithName("Value");

        return fluentValidator.Validate(context.Value).ToResult();
    }

    static abstract void SetupValidation<TFrom>(RuleBuilder<TFrom, TValue> ruleBuilder);
}

public sealed class IsConnectionString : IFluentValidationRule<IsConnectionString, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().MinimumLength(4);
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed class IsEmail : IFluentValidationRule<IsEmail, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.EmailAddress();
    }
}

public sealed class IsUrl : IFluentValidationRule<IsUrl, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.IsUrl();
    }
}

public sealed class IsJsonArrayOf<TMethod> : IFluentValidationRule<IsJsonArrayOf<TMethod>, string>
    where TMethod : IValidate<string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.Custom((array, context) =>
        {
            //Convert property to json
            var properties = JsonSerializer.Deserialize<IReadOnlyCollection<string>>(array);
            
            if (properties is null)
            {
                context.AddFailure(new ValidationFailure("JsonArray", "Could not deserialize"));
                return;
            }

            FluentValidator.ValidateCollection<TFrom, TMethod, string>(properties, context);
        });
    }
}

public sealed class IsCommaArrayOf<TMethod> : IFluentValidationRule<IsCommaArrayOf<TMethod>, string>
    where TMethod : IValidate<string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.Custom((array, context) =>
        {
            var properties = array.Split(",");

            if (properties.Length == 0)
            {
                return;
            }

            FluentValidator.ValidateCollection<TFrom, TMethod, string>(properties, context);
        });
    }
}

public sealed class IsNotEmpty : IFluentValidationRule<IsNotEmpty, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().NotNull();
    }
}

public sealed class IsNotNull<TValue> : IFluentValidationRule<IsNotNull<TValue>, TValue>
    where TValue : notnull
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, TValue> ruleBuilder)
    {
        ruleBuilder.NotNull();
    }
}