using System.Text.Json;

namespace Explicit.Validation.FluentValidation;

public interface IValidationRule<in TValue>
{
    public static abstract void SetupRule<TFrom>(IRuleBuilder<TFrom, TValue> builder);
}

public sealed class IsConnectionString : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed class IsEmail : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed class IsUrl : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.IsUrl());
    }
}

public sealed class IsJsonArrayOf<TMethod> : IValidate<string>
    where TMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.Custom((array, context) =>
        {
            //Convert property to json
            var properties = JsonSerializer.Deserialize<IReadOnlyCollection<string>>(array);
            
            if (properties is null)
            {
                context.AddFailure(new ValidationFailure("JsonArray", "Could not deserialize"));
                return;
            }

            context.ValidateCollection<TMethod, string>(properties);
        }));
    }
}

public sealed class IsCommaArrayOf<TMethod> : IValidate<string>
    where TMethod : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.Custom((array, context) =>
        {
            var properties = array.Split(",");

            if (properties.Length == 0)
            {
                return;
            }

            context.ValidateCollection<TMethod, string>(properties);
        }));
    }
}

public sealed class IsNotEmpty : IValidate<string>
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotEmpty().NotNull());
    }
}

public sealed class IsNotNull<TValue> : IValidate<TValue>
    where TValue : notnull
{
    public static OneOf<Success, ValidationErrors> Validate(Validator<TValue> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotNull());
    }
}