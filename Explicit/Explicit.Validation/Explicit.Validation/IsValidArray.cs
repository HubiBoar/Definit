using Explicit.Validation.Abstraction;

namespace Explicit.Validation;

public sealed class IsValidArray<T> : OneOfBase<ValidArray<T>, ValidationErrors>, IValidatable
    where T : IValidatable
{
    private IsValidArray(ValidArray<T> value)
        : base(value)
    {
    }

    private IsValidArray(ValidationErrors value)
        : base(value)
    {
    }

    public static IsValidArray<T> Create(IEnumerable<T> validatableCollection)
    {
        var request = validatableCollection.ToArray();

        var results = request.Select(validatable =>
        {
            var validationResult = validatable.Validate();

            return validationResult.Match<OneOf<Success, ValidationErrors>>(
                success => success,
                errors => errors);
        });

        var errors = results.Where(x => x.IsT1).Select(x => x.AsT1).ToArray();

        if (errors.Length > 0)
        {
            return new IsValidArray<T>(new ValidationErrors(errors));
        }

        return new IsValidArray<T>(new ValidArray<T>(request));
    }

    public OneOf<Success, ValidationErrors> Validate()
    {
        return Match<OneOf<Success, ValidationErrors>>(success => new Success(), errors => errors);
    }
}

public class ValidArray<T>
{
    public IReadOnlyCollection<T> Value { get; }

    internal ValidArray(IReadOnlyCollection<T> value)
    {
        Value = value;
    }

    public static explicit operator List<T>(ValidArray<T> valid)
    {
        return valid.Value.ToList();
    }

    public static explicit operator Valid(ValidArray<T> valid)
    {
        return new Valid();
    }
}