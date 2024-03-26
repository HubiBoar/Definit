namespace Definit.NewResults;

public static class NewResult
{
    public interface IError
    {
    }

    public static Func<TIn, Task<TOut>> ToTask<TIn, TOut>(this Func<TIn, TOut> func) => v => Task.FromResult(func(v));
    public static Func<TIn, Task> ToTask<TIn>(this Action<TIn> action) => v => { action(v); return Task.CompletedTask; };

    public class OneOf<T0, T1> : IOneOfT0<T0, T1>
        where T0 : notnull
        where T1 : notnull
    {
        public Type   Current { get; }
        public object  Object { get; }

        public OneOf(T0 value)
        {
            Object  = value;
            Current = value.GetType();
        }

        public OneOf(T1 value)
        {
            Object  = value;
            Current = value.GetType();
        }

        public static implicit operator OneOf<T0, T1>(T0 value) => new (value);
        public static implicit operator OneOf<T0, T1>(T1 value) => new (value);
        public static implicit operator OneOf<T0, T1>(OneOf<T1, T0> value) => value.Match<OneOf<T0, T1>>(t => t, t => t);

        public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1) => Current switch
        {
            T0 => onT0((T0)Object),
            _  => onT1((T1)Object)
        };
        
        public void Switch(Action<T0> onT0, Action<T1> onT1) => Match(v => { onT0(v); return Object; }, v => { onT1(v); return Object; });

        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, Task<T>> onT1) => Match<Task<T>>(onT0, onT1);

        public Task<T> Match<T>(Func<T0, T> onT0, Func<T1, Task<T>> onT1) => Match<Task<T>>(onT0.ToTask(), onT1);
        public Task<T> Match<T>(Func<T0, Task<T>> onT0, Func<T1, T> onT1) => Match<Task<T>>(onT0, onT1.ToTask());

        public Task Switch<T>(Func<T0, Task> onT0, Func<T1, Task> onT1) => Match(onT0, onT1);
        public Task Switch<T>(Func<T0, Task> onT0, Action<T1> onT1) => Match(onT0, onT1.ToTask());
        public Task Switch<T>(Action<T0> onT0, Func<T1, Task> onT1) => Match(onT0.ToTask(), onT1);
    }

    public class OneOf<T0, T1, T2> : IOneOf<T0, T1, T2>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
    {
        public Type Current  { get; }
        public object Object { get; }

        public OneOf(T0 value)
        {
            Object  = value;
            Current = value.GetType();
        }

        public OneOf(T1 value)
        {
            Object  = value;
            Current = value.GetType();
        }

        public OneOf(T2 value)
        {
            Object  = value;
            Current = value.GetType();
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

        public T Match<T>(Func<T0, T> onT0, Func<T1, T> onT1, Func<T2, T> onT2) =>  Current switch
        {
            T0 => onT0((T0)Object),
            T1 => onT1((T1)Object),
            _  => onT2((T2)Object),
        };

        public void Switch(Action<T0> onT0, Action<T1> onT1, Action<T2> onT2) => Match(v => { onT0(v); return Object; }, v => { onT1(v); return Object; }, v => { onT2(v); return Object; });


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
        public Type Current { get; } 
        public object Object { get; }

        public bool TryGetValue<T>(Type expected, out T value)
        {
            if(expected == Current)
            {
                value = (T)Convert.ChangeType(Object, Current);
                return true;
            }

            value = default!;
            return false;
        }
    }

    public interface IOneOfT0<T> : IOneOf {}
    public interface IOneOfT1<T> : IOneOf {}
    public interface IOneOfT2<T> : IOneOf {}
    public interface IOneOfT3<T> : IOneOf {}

    public static bool TryGetValue<T>(this IOneOfT0<T> oneOf, out T value) => oneOf.TryGetValue(typeof(T), out value);
    public static bool TryGetValue<T>(this IOneOfT1<T> oneOf, out T value) => oneOf.TryGetValue(typeof(T), out value);
    public static bool TryGetValue<T>(this IOneOfT2<T> oneOf, out T value) => oneOf.TryGetValue(typeof(T), out value);
    public static bool TryGetValue<T>(this IOneOfT3<T> oneOf, out T value) => oneOf.TryGetValue(typeof(T), out value);


    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);


    public static OneOfElse<T> Is<T, TElse>(this IOneOfT0<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT1<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfT2<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);


    // public static OneOf<T0, T1> As<T0, T1>(this IOneOfT1<OneOf<T0, T1>> oneOf)
    // {
    //     throw new Exception();
    // }

    // public static OneOf<T0, T1> As<T0, T1>(this IOneOfT2<OneOf<T0, T1>> oneOf)
    // {
    //     throw new Exception();
    // }

    // public static OneOf<T0, T1> As<T0, T1>(this IOneOfT3<OneOf<T0, T1>> oneOf)
    // {
    //     throw new Exception();
    // }

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
        if(oneOf.Is(out Value3 value2).Else(out var value1))
        {

        }

        if(oneOf.Is(out value1).Else(out value2))
        {

        }

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