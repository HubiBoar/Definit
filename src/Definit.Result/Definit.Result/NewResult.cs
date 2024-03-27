namespace Definit.NewResults;

public static class NewResult
{
    public interface IError {}

    public static Func<TIn, Task<TOut>> ToTask<TIn, TOut>(this Func<TIn, TOut> func) => v => Task.FromResult(func(v));
    public static Func<TIn, Task> ToTask<TIn>(this Action<TIn> action) => v => { action(v); return Task.CompletedTask; };

    public class OneOf<T0, T1> : IOneOfT0<T0, T1>
        where T0 : notnull
        where T1 : notnull
    {
        public int Index { get; }
        public object Value { get; }

        public OneOf(T0 value)
        {
            Index = 0;
            Value = value;
        }

        public OneOf(T1 value)
        {
            Index = 1;
            Value = value;
        }

        public static implicit operator OneOf<T0, T1>(T0 value) => new (value);
        public static implicit operator OneOf<T0, T1>(T1 value) => new (value);
        public static implicit operator OneOf<T0, T1>(OneOf<T1, T0> value) => value.Match<OneOf<T0, T1>>(t => t, t => t);

        public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1) => Index switch
        {
            0 => onT0((T0)Value),
            _ => onT1((T1)Value)
        };
        
        public void Switch(Action<T0> onT0, Action<T1> onT1) => Match(v => { onT0(v); return Value; }, v => { onT1(v); return Value; });

        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, Task<T>> onT1) => Match<Task<T>>(onT0, onT1);

        public Task<T> Match<T>(Func<T0, T> onT0, Func<T1, Task<T>> onT1) => Match<Task<T>>(onT0.ToTask(), onT1);
        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, T> onT1) => Match<Task<T>>(onT0, onT1.ToTask());

        public Task Switch<T>(Func<T0, Task> onT0, Func<T1, Task> onT1) => Match(onT0, onT1);
        public Task Switch<T>(Func<T0, Task> onT0, Action<T1> onT1) => Match(onT0, onT1.ToTask());
        public Task Switch<T>(Action<T0> onT0, Func<T1, Task> onT1) => Match(onT0.ToTask(), onT1);

        public bool TryGetValue(out T0 value) => IOneOf.TryGetValue(this, 0, out value);
        public bool TryGetValue(out T1 value) => IOneOf.TryGetValue(this, 1, out value);
    }

    public class OneOf<T0, T1, T2> : IOneOf<T0, T1, T2>
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

        public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t);

        public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T2, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T2, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T1, T0, T2> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T1, T0> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);
        public static implicit operator OneOf<T0, T1, T2>(OneOf<T2, T0, T1> value) => value.Match<OneOf<T0, T1, T2>>(t => t, t => t, t => t);

        public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1, Func<T2, T> onT2) => Index switch
        {
            0 => onT0((T0)Value),
            1 => onT1((T1)Value),
            _ => onT2((T2)Value),
        };

        public void Switch(Action<T0> onT0, Action<T1> onT1, Action<T2> onT2) => Match(v => { onT0(v); return Value; }, v => { onT1(v); return Value; }, v => { onT2(v); return Value; });


        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, Task<T>> onT1, Func<T2, Task<T>> onT2) => Match<Task<T>>(onT0, onT1, onT2);

        public Task<T> Match<T>(Func<T0, T> onT0, Func<T1, Task<T>> onT1, Func<T2, Task<T>> onT2) => Match<Task<T>>(onT0.ToTask(), onT1, onT2);
        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, T> onT1, Func<T2, Task<T>> onT2) => Match<Task<T>>(onT0, onT1.ToTask(), onT2);
        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, Task<T>> onT1, Func<T2, T> onT2) => Match<Task<T>>(onT0, onT1, onT2.ToTask());

        public Task<T> Match<T>(Func<T0, T> onT0, Func<T1, T> onT1, Func<T2, Task<T>> onT2) => Match<Task<T>>(onT0.ToTask(), onT1.ToTask(), onT2);
        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, T> onT1, Func<T2, T> onT2) => Match<Task<T>>(onT0, onT1.ToTask(), onT2.ToTask());
        public Task<T> Match<T>(Func<T0, T> onT0, Func<T1, Task<T>> onT1, Func<T2, T> onT2) => Match<Task<T>>(onT0.ToTask(), onT1, onT2.ToTask());


        public Task Switch<T>(Func<T0, Task> onT0, Func<T1, Task> onT1, Func<T2, Task> onT2) => Match(onT0, onT1, onT2);

        public Task Switch<T>(Action<T0> onT0, Func<T1, Task> onT1, Func<T2, Task> onT2) => Match(onT0.ToTask(), onT1, onT2);
        public Task Switch<T>(Func<T0, Task> onT0, Action<T1> onT1, Func<T2, Task> onT2) => Match(onT0, onT1.ToTask(), onT2);
        public Task Switch<T>(Func<T0, Task> onT0, Func<T1, Task> onT1, Action<T2> onT2) => Match(onT0, onT1, onT2.ToTask());

        public Task Switch<T>(Action<T0> onT0, Action<T1> onT1, Func<T2, Task> onT2) => Match(onT0.ToTask(), onT1.ToTask(), onT2);
        public Task Switch<T>(Func<T0, Task> onT0, Action<T1> onT1, Action<T2> onT2) => Match(onT0, onT1.ToTask(), onT2.ToTask());
        public Task Switch<T>(Action<T0> onT0, Func<T1, Task> onT1, Action<T2> onT2) => Match(onT0.ToTask(), onT1, onT2.ToTask());

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

    public interface IOneOf<T0, T1, T2> : IOneOfT0<T0, OneOf<T1, T2>>, IOneOfT1<T1, OneOf<T0, T2>>, IOneOfT2<T2, OneOf<T0, T1>>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
    {
    }

    public interface IOneOfT0<T, TElse> : IOneOfT0<T>, IOneOfT1<TElse> {}
    public interface IOneOfT1<T, TElse> : IOneOfT1<T>, IOneOfT2<TElse> {}
    public interface IOneOfT2<T, TElse> : IOneOfT2<T>, IOneOfT3<TElse> {}

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


    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);


    public static OneOfElse<T> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);

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

    public record Value1;
    public record Value2;
    public record Value3;
    public record Value4;

    private static OneOf<Value1, Value2> Example()
    {
        return new Value1();
    }

    private static OneOf<Value1, Value2, Value3> Example2(OneOf<Value1, Value3> oneOf)
    {
        return oneOf;
    }

    
    private static void Example3(OneOf<Value1, Value2, Value3> oneOf)
    {
        if(oneOf.Is(out Value2 value2).Else(out var value1))
        {

        }

        if(oneOf.Is(out value1).Else(out value2))
        {

        }
    }
}