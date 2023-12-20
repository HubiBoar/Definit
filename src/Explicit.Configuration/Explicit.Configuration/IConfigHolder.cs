using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigHolder<out TSection>
    where TSection : ISectionName
{
    internal IConfiguration Configuration { get; }
}

internal sealed class ConfigHolder<TSection> : IConfigHolder<TSection>
    where TSection : ISectionName
{
    public IConfiguration Configuration { get; }

    public ConfigHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}