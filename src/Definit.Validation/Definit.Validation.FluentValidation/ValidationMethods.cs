using System.Text.Json;
using Definit.Primitives;

namespace Definit.Validation.FluentValidation;

public sealed record IsConnectionString : ValidationMethod<IsConnectionString, string>
{
    protected override ValidationResult Validation(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed record IsEmail : ValidationMethod<IsEmail, string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed record IsUrl : ValidationMethod<IsUrl, string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.IsUrl());
    }
}

public sealed record IsJsonArrayOf<TMethod> : ValidationMethod<IsJsonArrayOf<TMethod>, string>
    where TMethod : IValidate<string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
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

public sealed record IsCommaArrayOf<TMethod> : ValidationMethod<IsCommaArrayOf<TMethod>, string>
    where TMethod : IValidate<string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
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

public sealed record IsNotEmpty : ValidationMethod<IsNotEmpty, string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotEmpty().NotNull());
    }
}

public sealed record IsNotNull<TValue> : ValidationMethod<IsNotNull<TValue>, TValue>
    where TValue : notnull
{
    protected override ValidationResult Validation(Validator<TValue> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotNull());
    }
}