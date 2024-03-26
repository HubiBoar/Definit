namespace Definit.NewResults;

public static class NewResult
{
    public interface IError
    {
    }

    public class OneOf<T0, T1> : IOneOfWithElseT0<T0, T1>
        where T0 : notnull
        where T1 : notnull
    {
        public Type[] Types { get; } = [ typeof(T0), typeof(T1) ];

        public int Current { get; }
        public object Object { get; }

        public OneOf(T0 value)
        {
            Object = value;
            Current = 0;
        }

        public OneOf(T1 value)
        {
            Object = value;
            Current = 1;
        }

        public static implicit operator OneOf<T0, T1>(T0 value)
        {
            return new (value);
        }

        public static implicit operator OneOf<T0, T1>(T1 value)
        {
            return new (value);
        }
    }

    public class OneOf<T0, T1, T2> : IOneOfWithElseT0<T0, OneOf<T1, T2>>, IOneOfWithElseT1<T1, OneOf<T0, T2>>, IOneOfWithElseT2<T2, OneOf<T0, T1>>
        where T0 : notnull
        where T1 : notnull
        where T2 : notnull
    {
        public Type[] Types { get; } = [ typeof(T0), typeof(T1), typeof(T2) ];

        public int Current { get; }
        public object Object { get; }

        public OneOf(T0 value)
        {
            Object = value;
            Current = 0;
        }

        public OneOf(T1 value)
        {
            Object = value;
            Current = 1;
        }

        public OneOf(T2 value)
        {
            Object = value;
            Current = 2;
        }

        public static implicit operator OneOf<T0, T1, T2>(T0 value) => new (value);
        public static implicit operator OneOf<T0, T1, T2>(T1 value) => new (value);
        public static implicit operator OneOf<T0, T1, T2>(T2 value) => new (value);

        public static implicit operator OneOf<T0, T1, T2>(OneOf<T0, T1> value) => new (value);
    }

    public interface IOneOfWithElseT0<T, TElse> : IOneOfT0<T>, IOneOfT1<TElse>
    {
    }

    public interface IOneOfWithElseT1<T, TElse> : IOneOfT1<T>, IOneOfT2<TElse>
    {
    }

    public interface IOneOfWithElseT2<T, TElse> : IOneOfT2<T>, IOneOfT3<TElse>
    {
    }

    public interface IOneOf
    {
        public Type[] Types { get; }

        public int Current { get; } 
        public object Object { get; }

        public bool TryGetValue<T>(int expected, out T value)
        {
            if(expected == Current)
            {
                value = (T)Convert.ChangeType(Object, Types[Current]);
                return true;
            }

            value = default!;
            return false;
        }
    }

    public interface IOneOfT0<T> : IOneOf
    {
    }

    public interface IOneOfT1<T> : IOneOf
    {
    }

    public interface IOneOfT2<T> : IOneOf
    {
    }

    public interface IOneOfT3<T> : IOneOf
    {
    }

    public static bool TryGetValue<T>(this IOneOfT0<T> oneOf, out T value) => oneOf.TryGetValue(0, out value);
    public static bool TryGetValue<T>(this IOneOfT1<T> oneOf, out T value) => oneOf.TryGetValue(1, out value);
    public static bool TryGetValue<T>(this IOneOfT2<T> oneOf, out T value) => oneOf.TryGetValue(2, out value);
    public static bool TryGetValue<T>(this IOneOfT3<T> oneOf, out T value) => oneOf.TryGetValue(3, out value);


    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfWithElseT0<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfWithElseT1<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);
    public static OneOfElse<TElse> Is<T, TElse>(this IOneOfWithElseT2<T, TElse> oneOf, out T value) => new (oneOf.TryGetValue(out value), oneOf.TryGetValue);


    public static OneOfElse<T> Is<T, TElse>(this IOneOfWithElseT0<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfWithElseT1<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);
    public static OneOfElse<T> Is<T, TElse>(this IOneOfWithElseT2<T, TElse> oneOf, out TElse value) => new (oneOf.TryGetValue(out value) is false, oneOf.TryGetValue);


    // public static OneOf<T0, T1> As<T0, T1>(this IOneOfT0<T> oneOf)
    // {
    //     throw new Exception();
    // }

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

    private static OneOf<Value1, Value2> Example()
    {
        return new Value1();
    }

    private static OneOf<Value1, Value2, Value3> Example2(OneOf<Value1, Value2> oneOf)
    {
        if(oneOf.Is(out Value2 value2).Else(out var value1))
        {

        }

        if(oneOf.Is(out value1).Else(out value2))
        {

        }

        throw new Exception();
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