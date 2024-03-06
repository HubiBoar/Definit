using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Momolith.Modules;

namespace Definit.Endpoint;

public static class EndpointExtensions
{
    public static IEndpointConventionBuilder Map<T>(this IEndpointRouteBuilder endpoint)
        where T : IEndpointProvider
    {
        return T.Endpoint.Map(endpoint);
    }

    public static IHostExtender<WebApplication> Map<T>(this IHostExtender<WebApplication> extender)
        where T : IEndpointProvider
    {
        extender.Map<T>();

        return extender;
    }

    public static IHostExtender<WebApplication> Map(this IHostExtender<WebApplication> extender, Endpoint endpoint)
    {
        extender.Extend(host => endpoint.Map(host));

        return extender;
    }
}