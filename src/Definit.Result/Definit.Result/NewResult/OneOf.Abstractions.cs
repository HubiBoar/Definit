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

public interface IOneOf<T0, T1> : IOneOfT0<T0, T1>
{
    public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1);
    public void Switch(Action<T0> onT0, Action<T1> onT1);
}

public interface IOneOf<T0, T1, T2> : IOneOfT0<T0>, IOneOfT1<T1>, IOneOfT2<T2>
{
    public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1, Func<T2, T> onT2);
    public void Switch(Action<T0> onT0, Action<T1> onT1, Action<T2> onT2);
}