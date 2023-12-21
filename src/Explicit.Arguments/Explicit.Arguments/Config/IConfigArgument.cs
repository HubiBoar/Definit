using Explicit.Configuration;
using Microsoft.Extensions.Configuration;

namespace Explicit.Arguments.Config;

public interface IConfigArgument
{
    protected IConfiguration Configuration { get; }
}

public interface IConfigArgument<TSection, TValue> : IConfigArgument, IArgumentProvider<TValue>
    where TSection :  IConfigObject<TSection>
{
    TValue IArgumentProvider<TValue>.GetValue()
    {
        return Configuration.GetValid<TSection>().Basic.Match(
            Convert,
            errors => throw errors.ToException());
    }
    
    TValue Convert(TSection option);
}