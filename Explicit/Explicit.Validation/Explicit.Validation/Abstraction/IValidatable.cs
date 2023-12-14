namespace Explicit.Validation.Abstraction;

public interface IValidatable
{
    public OneOf<Success, ValidationErrors> Validate();
}