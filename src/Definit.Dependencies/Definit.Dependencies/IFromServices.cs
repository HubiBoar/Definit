using Microsoft.Extensions.DependencyInjection;

namespace Definit.Dependencies;

public interface IFromServices<TSelf>
    where TSelf : class, IFromServices<TSelf>
{
    public abstract static Type[] Types { get; }

    public static abstract TSelf Create(IServiceProvider provider);
}