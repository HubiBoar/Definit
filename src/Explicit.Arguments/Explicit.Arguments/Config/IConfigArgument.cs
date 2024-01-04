using Explicit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explicit.Arguments.Config;

public interface IConfigArgument
{
    protected IConfiguration Configuration { get; }

    protected IServiceCollection Services { get; }
}

public interface IConfigArgument<TSection, TValue> : IConfigArgument, IArgumentProvider<TValue>
    where TSection :  IConfigObject<TSection>
{
    TValue IArgumentProvider<TValue>.GetValue()
    {
        return Configuration.GetValid<TSection>(Services).Basic.Match(
            Convert,
            errors => throw errors.ToException());
    }
    
    TValue Convert(TSection option);
}