namespace Definit.Endpoint;

public interface IEndpointProvider
{
    public static abstract Endpoint Endpoint { get; }
}