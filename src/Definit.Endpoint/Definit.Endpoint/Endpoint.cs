using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Momolith.Modules;

namespace Definit.Endpoint;

public sealed class Endpoint : IEndpointConventionBuilder
{
    private readonly Func<IEndpointRouteBuilder, IEndpointConventionBuilder> _extender;
    private readonly List<Action<EndpointBuilder>> _conventions = new ();

    public Endpoint(Func<IEndpointRouteBuilder, IEndpointConventionBuilder> extender)
    {
        _extender = extender;
    }

    public void Add(Action<EndpointBuilder> convention)
    {
        _conventions.Add(convention);
    }

    internal IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint)
    {
        var builder = _extender(endpoint);
        foreach(var convention in _conventions)
        {
            builder.Add(convention);
        }

        return builder;
    }
}

public interface IEndpointProvider
{
    public static abstract Endpoint Endpoint { get; }
}