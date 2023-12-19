using System.Text.Json;

namespace Explicit.Validation.NewFluent.Fluent;

public sealed class IsConnectionString : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().MinimumLength(4);
    }
}

public sealed class IsEmail : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.EmailAddress();
    }
}

public sealed class IsUrl : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.IsUrl();
    }
}

public sealed class IsJsonArrayOf<TMethod> : IValidationRule<string>
    where TMethod : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
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

            context.Use<TFrom, TMethod, string>(properties);
        });
    }
}

public sealed class IsCommaArrayOf<TMethod> : IValidationRule<string>
    where TMethod : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.Custom((array, context) =>
        {
            var properties = array.Split(",");

            if (properties.Length == 0)
            {
                return;
            }

            context.Use<TFrom, TMethod, string>(properties);
        });
    }
}

public sealed class IsNotEmpty : IValidationRule<string>
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, string> ruleBuilder)
    {
        ruleBuilder.NotEmpty().NotNull();
    }
}

public sealed class IsNotNull<TValue> : IValidationRule<TValue>
    where TValue : notnull
{
    public static void SetupRule<TFrom>(IRuleBuilder<TFrom, TValue> ruleBuilder)
    {
        ruleBuilder.NotNull();
    }
}