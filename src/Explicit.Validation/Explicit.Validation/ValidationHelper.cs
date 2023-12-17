namespace Explicit.Validation;

public static class ValidationHelper
{
    public static IReadOnlyCollection<OneOf<Success, ValidationErrors>> ValidateCollection<TValue, TMethod>(
        IReadOnlyCollection<TValue> collection)
        where TMethod : IValidate<TValue>
        where TValue : notnull
    {
        return collection.Select(x => TMethod.Validate(new Validator<TValue>(x))).ToArray();
    }
}