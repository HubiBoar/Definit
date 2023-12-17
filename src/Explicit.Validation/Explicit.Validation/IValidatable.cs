namespace Explicit.Validation;

public interface IValidatable
{
    public OneOf<Success, ValidationErrors> Validate();
}