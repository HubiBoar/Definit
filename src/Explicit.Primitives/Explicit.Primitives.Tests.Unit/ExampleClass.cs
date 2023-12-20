using Explicit.Validation;
using Explicit.Validation.FluentValidation;
using FluentValidation;
using OneOf;
using OneOf.Types;

namespace Explicit.Primitives.Tests.Unit;

internal class ExampleClass : IValidate<ExampleClass>
{
    public Value<string, IsConnectionString> ConnectionString { get; }
    
    public Value<string, IsEmail> Email { get; }

    public ExampleClass(string connectionString, string email)
    {
        ConnectionString = connectionString;
        Email = email;
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.ConnectionString)
                .NotEmpty();

            validator.RuleFor(x => x.Email)
                .NotEmpty();
        });
    }
}