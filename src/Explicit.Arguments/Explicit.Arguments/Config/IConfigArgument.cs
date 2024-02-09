using Explicit.Configuration;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explicit.Arguments.Config;

public interface IConfigArgument
{
    protected IConfiguration Configuration { get; }

    protected IServiceCollection Services { get; }
}

public interface IConfigArgument<TSection, TValue> : IConfigArgument, IArgumentProvider<TValue>
    where TSection : IConfigObject<TSection>, IValidate<TSection>
{
    TValue IArgumentProvider<TValue>.GetValue()
    {
        return TSection.Create(Configuration).Basic.Match(
            Convert,
            errors => throw errors.ToException());
    }
    
    TValue Convert(TSection option);
}