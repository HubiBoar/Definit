using Definit.Results;

namespace Definit.Validation;

public sealed class ValidationResult : ResultBase<Success, ValidationErrors>
{
    public static ValidationResult Success { get; } = new (Results.Success.Instance);

    public ValidationResult(Success value)          : base(value) {}
    public ValidationResult(ValidationErrors value) : base(value) {}
    public ValidationResult(Error error)            : base(error) {}
    public static implicit operator ValidationResult(Success value)          => new (value);
    public static implicit operator ValidationResult(ValidationErrors value) => new (value);
    public static implicit operator ValidationResult(Error value)            => new (value);
    public static implicit operator ValidationResult(Exception value)        => new (value);

    public static implicit operator ValidationResult(OneOf<Success, Error> value)            => value.Match<ValidationResult>(x => x, x => x);
    public static implicit operator ValidationResult(OneOf<Error, ValidationErrors> value)   => value.Match<ValidationResult>(x => x, x => x);
    public static implicit operator ValidationResult(OneOf<ValidationErrors, Error> value)   => value.Match<ValidationResult>(x => x, x => x);
    public static implicit operator ValidationResult(OneOf<Success, ValidationErrors> value) => value.Match<ValidationResult>(x => x, x => x);

    public static implicit operator Result(ValidationResult value)           => value.Match<Result>(x => Results.Success.Instance, x => Results.Success.Instance, x => x);

    public static implicit operator Task<ValidationResult>(ValidationResult value)  => Task.FromResult(value);
}