namespace Definit.NewResultsSafe;

public static class NewResultSafe
{
    public interface IOneOfT0<T, TElse> : IOneOfT0<T>, IOneOfT1<TElse> {}
    public interface IOneOfT1<T, TElse> : IOneOfT1<T>, IOneOfT2<TElse> {}
    public interface IOneOfT2<T, TElse> : IOneOfT2<T>, IOneOfT3<TElse> {}
    
    public interface IOneOfT0<T> : IOneOf
    {
        bool TryGetValue(out T value);
    }
    public interface IOneOfT1<T> : IOneOf
    {
        bool TryGetValue(out T value);
    }
    public interface IOneOfT2<T> : IOneOf
    {
        bool TryGetValue(out T value);
    }
    public interface IOneOfT3<T> : IOneOf
    {
        bool TryGetValue(out T value);
    }

    public interface IOneOf
    {
        public Type Current { get; } 
        public object Object { get; }
    }

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

        public bool TryGetValue(out T0 value) => 
        public bool TryGetValue(out T1 value) => 
    }


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
}