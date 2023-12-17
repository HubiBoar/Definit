namespace Explicit.Validation;

public static class ValidationHelper
{
    public static IReadOnlyCollection<OneOf<Success, ValidationErrors>> ValidateCollection<TValue, TMethod>(
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidationMethod<TValue>
    {
        return collection.Select(TMethod.Validate).ToArray();
    }
}