using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public sealed class FromServicesProvider
{
    public IServiceProvider Provider {get;}

    public FromServicesProvider(IServiceProvider provider)
    {
        Provider = provider;
    }

    public static void Register(IServiceCollection services)
    {
        services.AddSingleton(provider => new FromServicesProvider(provider));
        services.AddSingleton(typeof(FromServices<>));
        services.AddSingleton(typeof(FromServices<,>));
        services.AddSingleton(typeof(FromServices<,,>));
        services.AddSingleton(typeof(FromServices<,,,>));
        services.AddSingleton(typeof(FromServices<,,,,>));
        services.AddSingleton(typeof(FromServices<,,,,,>));
    }
}

public static class FromServicesExtensions
{
    public static IServiceCollection AddFromServices(this IServiceCollection service)
    {
        FromServicesProvider.Register(service);
        return service;
    }

    public static FromServicesProvider From(this IServiceProvider provider)
    {
        return new FromServicesProvider(provider);
    }
}