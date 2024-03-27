namespace Definit.NewResults;

public interface IOneOf
{
    public int Index    { get; } 
    public object Value { get; }

    public static bool TryGetValue<T>(IOneOf oneOf, int expected, Func<object, T> cast, out T value)
    {
        if(oneOf.Index == expected)
        {
            value = cast(oneOf.Value);
            return true;
        }

        value = default!;
        return false;
    }

    public static bool TryGetValue<T>(IOneOf oneOf, int expected, out T value) => TryGetValue(oneOf, expected, v => (T)v, out value);
}

public interface IOneOfT0<T> : IOneOf { bool TryGetValue(out T value); }
public interface IOneOfT1<T> : IOneOf { bool TryGetValue(out T value); }
public interface IOneOfT2<T> : IOneOf { bool TryGetValue(out T value); }
public interface IOneOfT3<T> : IOneOf { bool TryGetValue(out T value); }

public interface IOneOfT0<T, TElse> : IOneOfT0<T>, IOneOfT1<TElse> {}
public interface IOneOfT1<T, TElse> : IOneOfT1<T>, IOneOfT2<TElse> {}
public interface IOneOfT2<T, TElse> : IOneOfT2<T>, IOneOfT3<TElse> {}


public interface IOneOf<T0, T1, T2> : IOneOfT0<T0, OneOf<T1, T2>>, IOneOfT1<T1, OneOf<T0, T2>>, IOneOfT2<T2, OneOf<T0, T1>>
    where T0 : notnull
    where T1 : notnull
    where T2 : notnull {}