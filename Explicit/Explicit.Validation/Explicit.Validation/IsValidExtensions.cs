namespace Explicit.Validation;

public static class IsValidExtensions
{
    public static IsValid<T> IsValid<T>(this T request)
        where T : IValidatable
    {
        return Validation.IsValid<T>.Create(request);
    }
    
    public static IsValidArray<T> IsValid<T>(this IEnumerable<T> requests)
        where T : IValidatable
    {
        return IsValidArray<T>.Create(requests);
    }
}