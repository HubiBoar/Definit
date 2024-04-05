using Definit.Primitives;
using Definit.Validation.FluentValidation;
using FluentValidation;

namespace Definit.Validation.Tests.Unit.Fluent;

public sealed class IsValue0 : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MaximumLength(2));
    }
}

public sealed class IsValue1 : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(3).MaximumLength(3));
    }
}

internal class ExampleClass : IValidate<ExampleClass>
{
    public required Value<string, IsValue0> Value0 { get; init; }
    
    public required Value<string, IsValue1> Value1 { get; init; }

    public required Value<string, IsCommaArrayOf<IsValue0>> CommaArray { get; init; }

    public static ValidationResult Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value0).ValidateSelf();

            validator.RuleFor(x => x.Value1).ValidateSelf();

            validator.RuleFor(x => x.CommaArray).ValidateSelf();
        });
    }
}