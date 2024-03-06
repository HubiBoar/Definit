using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Definit.Endpoint;

public static class Map
{
    public static Endpoint Get(string pattern, Delegate handler)
    {
        return new Endpoint(endpoint => endpoint.MapGet(pattern, handler));
    }

    public static Endpoint Post(string pattern, Delegate handler)
    {
        return new Endpoint(endpoint => endpoint.MapPost(pattern, handler));
    }

    public static Endpoint Put(string pattern, Delegate handler)
    {
        return new Endpoint(endpoint => endpoint.MapPut(pattern, handler));
    }

    public static Endpoint Delete(string pattern, Delegate handler)
    {
        return new Endpoint(endpoint => endpoint.MapDelete(pattern, handler));
    }
}