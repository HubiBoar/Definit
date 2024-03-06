using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public sealed class FromServices<T0, T1, T2, T3> : IFromServices<FromServices<T0, T1, T2, T3>>
    where T0 : class
    where T1 : class
    where T2 : class
    where T3 : class
{
    public static Type[] Types { get; } = [ typeof(T0), typeof(T1), typeof(T2) , typeof(T3) ];

    public T0 Value0 { get; }

    public T1 Value1 { get; }

    public T2 Value2 { get; }

    public T3 Value3 { get; }

    public FromServices(T0 value0, T1 value1, T2 value2, T3 value3)
    {
        Value0 = value0;
        Value1 = value1;
        Value2 = value2;
        Value3 = value3;
    }

    public static FromServices<T0, T1, T2, T3> Create(IServiceProvider provider)
    {
        return new FromServices<T0, T1, T2, T3>(
            provider.GetRequiredService<T0>(),
            provider.GetRequiredService<T1>(),
            provider.GetRequiredService<T2>(),
            provider.GetRequiredService<T3>());
    }

    public void Deconstruct(
        out T0 value0,
        out T1 value1,
        out T2 value2,
        out T3 value3)
    {
        value0 = Value0;
        value1 = Value1;
        value2 = Value2;
        value3 = Value3;
    }
}