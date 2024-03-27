namespace Definit.NewResults;

public static class HelperExtensions
{
    public static Func<TIn, Task<TOut>> ToTask<TIn, TOut>(this Func<TIn, TOut> func) => v => Task.FromResult(func(v));
    public static Func<TIn, Task> ToTask<TIn>(this Action<TIn> action) => v => { action(v); return Task.CompletedTask; };
}