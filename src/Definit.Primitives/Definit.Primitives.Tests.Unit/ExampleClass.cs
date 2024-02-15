using Definit.Validation;
using Definit.Validation.FluentValidation;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace Definit.Primitives.Tests.Unit;

internal class ExampleClass : IValidate<ExampleClass>
{
    public required Value<string, IsConnectionString> ConnectionString { get; init; }
    
    public required Value<string, IsEmail> Email { get; init; }

    public static OneOf<Success, ValidationErrors> Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.ConnectionString).ValidateSelf();

            validator.RuleFor(x => x.Email).ValidateSelf();
        });
    }
}