using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Momolith.Modules;

namespace Definit.Endpoint;

public interface IEndpointProvider
{
    public static abstract Endpoint Endpoint { get; }
}