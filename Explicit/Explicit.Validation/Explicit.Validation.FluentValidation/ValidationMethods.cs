using System.Text.Json;
using Explicit.Validation;

namespace Explicit.Validation.FluentValidation;

public sealed class IsConnectionString : IFluentValidationMethodSelf<IsConnectionString, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().MinimumLength(4);
    }
}

public sealed class IsEmail : IFluentValidationMethodSelf<IsEmail, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.EmailAddress();
    }
}

public sealed class IsUrl : IFluentValidationMethodSelf<IsUrl, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.IsUrl();
    }
}

public sealed class IsJsonArrayOf<TMethod> : IFluentValidationMethodSelf<IsJsonArrayOf<TMethod>, string>
    where TMethod : IValidationMethod<string>
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

public sealed class IsCommaArrayOf<TMethod> : IFluentValidationMethodSelf<IsCommaArrayOf<TMethod>, string>
    where TMethod : IValidationMethod<string>
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

public sealed class IsNotEmpty : IFluentValidationMethodSelf<IsNotEmpty, string>
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().NotNull();
    }
}

public sealed class IsNotNull<TValue> : IFluentValidationMethodSelf<IsNotNull<TValue>, TValue>
    where TValue : notnull
{
    public static void SetupValidation<TFrom>(RuleBuilder<TFrom, TValue> ruleBuilder)
    {
        ruleBuilder.NotNull();
    }
}