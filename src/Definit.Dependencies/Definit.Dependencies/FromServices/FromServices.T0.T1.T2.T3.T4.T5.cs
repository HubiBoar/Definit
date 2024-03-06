using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public sealed class FromServices<T0, T1, T2, T3, T4, T5> : IFromServices<FromServices<T0, T1, T2, T3, T4, T5>>
    where T0 : class
    where T1 : class
    where T2 : class
    where T3 : class
    where T4 : class
    where T5 : class
{
    public static Type[] Types { get; } = [ typeof(T0), typeof(T1), typeof(T2) , typeof(T3) , typeof(T4) , typeof(T5) ];

    public T0 Value0 { get; }

    public T1 Value1 { get; }

    public T2 Value2 { get; }

    public T3 Value3 { get; }

    public T4 Value4 { get; }

    public T5 Value5 { get; }

    public FromServices(T0 value0, T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        Value0 = value0;
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
        Value4 = value4;
        Value5 = value5;
    }

    public static FromServices<T0, T1, T2, T3, T4, T5> Create(IServiceProvider provider)
    {
        return new FromServices<T0, T1, T2, T3, T4, T5>(
            provider.GetRequiredService<T0>(),
            provider.GetRequiredService<T1>(),
            provider.GetRequiredService<T2>(),
            provider.GetRequiredService<T3>(),
            provider.GetRequiredService<T4>(),
            provider.GetRequiredService<T5>());
    }

    public void Deconstruct(
        out T0 value0,
        out T1 value1,
        out T2 value2,
        out T3 value3,
        out T4 value4,
        out T5 value5)
    {
        value0 = Value0;
        value1 = Value1;
        value2 = Value2;
        value3 = Value3;
        value4 = Value4;
        value5 = Value5;
    }
}