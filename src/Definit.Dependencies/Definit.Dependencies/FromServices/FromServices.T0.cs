using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public sealed class FromServices<T0> : IFromServices<FromServices<T0>>
    where T0 : class
{
    public static Type[] Types { get; } = [ typeof(T0) ];

    public T0 Value0 { get; }

    public FromServices(T0 value0)
    {
        Value0 = value0;
    }

    public static FromServices<T0> Create(IServiceProvider provider)
    {
        return new FromServices<T0>(
            provider.GetRequiredService<T0>());
    }

    public static implicit operator FromServices<T0>(FromServicesProvider provider)
    {
        return Create(provider.Provider);
    }

    public void Deconstruct(out T0 value0)
    {
        value0 = Value0;
    }
}