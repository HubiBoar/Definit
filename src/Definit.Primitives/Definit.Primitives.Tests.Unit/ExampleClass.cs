using Definit.Validation;
using Definit.Validation.FluentValidation;
using OneOf;

namespace Definit.Primitives.Tests.Unit;

internal class ExampleClass : IValidate<ExampleClass>
{
    public required Value<string, IsConnectionString> ConnectionString { get; init; }
    
    public required Value<string, IsEmail> Email { get; init; }

    public static ValidationResult Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.ConnectionString).ValidateSelf();

            validator.RuleFor(x => x.Email).ValidateSelf();
        });
    }
}