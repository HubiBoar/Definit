namespace Definit.NewResultsOneOf;

public delegate bool TryGetValueDelegate<T>(out T value);

public sealed record OneOfElse<T>(bool IsTrue, TryGetValueDelegate<T> get)
{
    public bool Else(out T value)
    {
        get(out value);
        return IsTrue;
    }

    public static implicit operator bool(OneOfElse<T> oneOfElse)
    {
        return oneOfElse.IsTrue;
    }
}

public static class OneOfElseExtensions
{
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);


    public static OneOfElse<T> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
}