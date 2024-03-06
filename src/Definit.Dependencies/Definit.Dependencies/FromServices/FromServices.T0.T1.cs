using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public sealed class FromServices<T0, T1> : IFromServices<FromServices<T0, T1>>
    where T0 : class
    where T1 : class
{
    public static Type[] Types { get; } = [ typeof(T0), typeof(T1) ];

    public T0 Value0 { get; }

    public T1 Value1 { get; }

    public FromServices(T0 value0, T1 value1)
    {
        Value0 = value0;
        Value1 = value1;
    }

    public static FromServices<T0, T1> Create(IServiceProvider provider)
    {
        return new FromServices<T0, T1>(
            provider.GetRequiredService<T0>(),
            provider.GetRequiredService<T1>());
    }

    public static implicit operator FromServices<T0, T1>(FromServicesProvider provider)
    {
        return Create(provider.Provider);
    }

    public void Deconstruct(out T0 value0, out T1 value1)
    {
        value0 = Value0;
        value1 = Value1;
    }
}