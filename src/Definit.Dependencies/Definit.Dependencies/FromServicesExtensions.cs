using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public class FromServiceProvider
{
    public IServiceProvider Provider {get;}

    public FromServiceProvider(IServiceProvider provider)
    {
        Provider = provider;
    }
}

public static class ServiceProviderExtensions
{
    public static FromServiceProvider From(this IServiceProvider provider)
    {
        return new FromServiceProvider(provider);
    }
}