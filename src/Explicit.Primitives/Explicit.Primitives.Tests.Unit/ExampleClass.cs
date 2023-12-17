using Explicit.Validation.FluentValidation;

namespace Explicit.Primitives.Tests.Unit;

internal class ExampleClass : IFluentValidatable<ExampleClass>
{
    public Value<IsConnectionString> ConnectionString { get; }
    
    public Value<string, IsEmail> Email { get; }

    public ExampleClass(string connectionString, string email)
    {
        ConnectionString = connectionString;
        Email = email;
    }

    public static void SetupValidation(FluentValidator<ExampleClass> validator)
    {
        validator.RuleFor(x => x.ConnectionString)
            .ValidateSelf();
        
        validator.RuleFor(x => x.Email)
            .ValidateSelf();
    }
}