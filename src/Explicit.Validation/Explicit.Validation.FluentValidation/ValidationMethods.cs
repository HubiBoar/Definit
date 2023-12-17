using System.Text.Json;
using Explicit.Validation;

namespace Explicit.Validation.FluentValidation;

public sealed class IsConnectionString : IFluentValidationRuleMethod<IsConnectionString, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().MinimumLength(4);
    }
}

public sealed class IsEmail : IFluentValidationRuleMethod<IsEmail, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.EmailAddress();
    }
}

public sealed class IsUrl : IFluentValidationRuleMethod<IsUrl, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.IsUrl();
    }
}

public sealed class IsJsonArrayOf<TMethod> : IFluentValidationRuleMethod<IsJsonArrayOf<TMethod>, string>
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

public sealed class IsCommaArrayOf<TMethod> : IFluentValidationRuleMethod<IsCommaArrayOf<TMethod>, string>
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

public sealed class IsNotEmpty : IFluentValidationRuleMethod<IsNotEmpty, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().NotNull();
    }
}

public sealed class IsNotNull<TValue> : IFluentValidationRuleMethod<IsNotNull<TValue>, TValue>
    where TValue : notnull
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, TValue> ruleBuilder)
    {
        ruleBuilder.NotNull();
    }
}