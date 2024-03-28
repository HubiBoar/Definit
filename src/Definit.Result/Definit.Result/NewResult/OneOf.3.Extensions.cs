namespace Definit.NewResultsOneOf;

public static class OneOf3Extensions
{
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task<TOut>> onT0, Func<T1, Task<TOut>> onT1, Func<T2, Task<TOut>> onT2) => oneOf.Match(onT0, onT1, onT2);
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, TOut> onT0, Func<T1, Task<TOut>> onT1, Func<T2, Task<TOut>> onT2)       => oneOf.Match(onT0.ToTask(), onT1, onT2);
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task<TOut>> onT0, Func<T1, TOut> onT1, Func<T2, Task<TOut>> onT2)       => oneOf.Match(onT0, onT1.ToTask(), onT2);
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task<TOut>> onT0, Func<T1, Task<TOut>> onT1, Func<T2, TOut> onT2)       => oneOf.Match(onT0, onT1, onT2.ToTask());
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, TOut> onT0, Func<T1, TOut> onT1, Func<T2, Task<TOut>> onT2)             => oneOf.Match(onT0.ToTask(), onT1.ToTask(), onT2);
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task<TOut>> onT0, Func<T1, TOut> onT1, Func<T2, TOut> onT2)             => oneOf.Match(onT0, onT1.ToTask(), onT2.ToTask());
    public static Task<TOut> Match<TOut, T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, TOut> onT0, Func<T1, Task<TOut>> onT1, Func<T2, TOut> onT2)             => oneOf.Match(onT0.ToTask(), onT1, onT2.ToTask());


    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task> onT0, Func<T1, Task> onT1, Func<T2, Task> onT2) => oneOf.Match(onT0, onT1, onT2);
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Action<T0> onT0, Func<T1, Task> onT1, Func<T2, Task> onT2)     => oneOf.Match(onT0.ToTask(), onT1, onT2);
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task> onT0, Action<T1> onT1, Func<T2, Task> onT2)     => oneOf.Match(onT0, onT1.ToTask(), onT2);
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task> onT0, Func<T1, Task> onT1, Action<T2> onT2)     => oneOf.Match(onT0, onT1, onT2.ToTask());
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Action<T0> onT0, Action<T1> onT1, Func<T2, Task> onT2)         => oneOf.Match(onT0.ToTask(), onT1.ToTask(), onT2);
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Func<T0, Task> onT0, Action<T1> onT1, Action<T2> onT2)         => oneOf.Match(onT0, onT1.ToTask(), onT2.ToTask());
    public static Task Switch<T0, T1, T2>(this IOneOf<T0, T1, T2> oneOf, Action<T0> onT0, Func<T1, Task> onT1, Action<T2> onT2)         => oneOf.Match(onT0.ToTask(), onT1, onT2.ToTask());
}