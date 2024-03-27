namespace Definit.NewResults;

public class OneOf<T0, T1> : IOneOf<T0, T1>, IOneOfT0<T0, T1>
    where T0 : notnull
    where T1 : notnull
{
    public int Index { get; }
    public object Value { get; }

    protected OneOf(int index, object value) { Index = index; Value = value; }
    public OneOf(T0 value) : this(0, value) {}
    public OneOf(T1 value) : this(1, value) {}

    public static implicit operator OneOf<T0, T1>(T0 value) => new (value);
    public static implicit operator OneOf<T0, T1>(T1 value) => new (value);
    public static implicit operator OneOf<T0, T1>(OneOf<T1, T0> value) => value.Match<OneOf<T0, T1>>(t => t, t => t);

    public static implicit operator Task<OneOf<T0, T1>>(OneOf<T0, T1> value) => Task.FromResult(value);

    public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1) => Index switch
    {
        0 => onT0((T0)Value),
        _ => onT1((T1)Value)
    };
    
    public void Switch(Action<T0> onT0, Action<T1> onT1) => Match(v => { onT0(v); return Value; }, v => { onT1(v); return Value; });

    public bool TryGetValue(out T0 value) => IOneOf.TryGetValue(this, 0, out value);
    public bool TryGetValue(out T1 value) => IOneOf.TryGetValue(this, 1, out value);
}