namespace Definit.NewResults;

public interface IOneOfFull<T0, T1, T2> :  IOneOf<T0, T1, T2>, IOneOfT0<T0, OneOf<T1, T2>>, IOneOfT1<T1, OneOf<T0, T2>>, IOneOfT2<T2, OneOf<T0, T1>>
    where T0 : notnull
    where T1 : notnull
    where T2 : notnull {}

public class OneOf<T0, T1, T2> : IOneOfFull<T0, T1, T2>
    where T0 : notnull
    where T1 : notnull
    where T2 : notnull
{
    public int Index { get; }
    public object Value { get; }

    public OneOf(T0 value)
    {
        Value  = value;
        Index = 0;
    }

    public OneOf(T1 value)
    {
        Value  = value;
        Index = 1;
    }

    public OneOf(T2 value)
    {
        Value  = value;
        Index = 2;
    }

    public static implicit operator OneOf<T0, T1, T2>(T0 value) => new (value);
    public static implicit operator OneOf<T0, T1, T2>(T1 value) => new (value);
    public static implicit operator OneOf<T0, T1, T2>(T2 value) => new (value);

    public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);

    public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T2, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T2, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T0, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T1, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
    public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T0, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);

    public static implicit operator Task<OneOf<T0, T1, T2>>(OneOf<T0, T1, T2> value) => Task.FromResult(value);

    public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1, Func<T2, T> onT2) => Index switch
    {
        0 => onT0((T0)Value),
        1 => onT1((T1)Value),
        _ => onT2((T2)Value),
    };

    public void Switch(Action<T0> onT0, Action<T1> onT1, Action<T2> onT2) => Match(v => { onT0(v); return Value; }, v => { onT1(v); return Value; }, v => { onT2(v); return Value; });

    public bool TryGetValue(out T0 value) => IOneOf.TryGetValue(this, 0, out value);
    public bool TryGetValue(out T1 value) => IOneOf.TryGetValue(this, 1, out value);
    public bool TryGetValue(out T2 value) => IOneOf.TryGetValue(this, 2, out value);

    public bool TryGetValue(out OneOf<T0, T1> value)
    {
        if(IOneOf.TryGetValue(this, 0, v => (T0)v, out value))
        {
            return true;
        }

        return IOneOf.TryGetValue(this, 1, v => (T1)v, out value);
    }

    public bool TryGetValue(out OneOf<T0, T2> value)
    {
        if(IOneOf.TryGetValue(this, 0, v => (T0)v, out value))
        {
            return true;
        }

        return IOneOf.TryGetValue(this, 2, v => (T2)v, out value);
    }

    public bool TryGetValue(out OneOf<T1, T2> value)
    {
        if(IOneOf.TryGetValue(this, 1, v => (T1)v, out value))
        {
            return true;
        }

        return IOneOf.TryGetValue(this, 2, v => (T2)v, out value);
    }
}