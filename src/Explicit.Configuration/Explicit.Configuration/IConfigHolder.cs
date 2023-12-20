using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigHolder<out TSection>
    where TSection : ISectionName
{
    internal ConfigurationHolder Configuration { get; }
}

internal sealed class ConfigHolder<TSection> : IConfigHolder<TSection>
    where TSection : ISectionName
{
    public ConfigurationHolder Configuration { get; }

    public ConfigHolder(ConfigurationHolder configuration)
    {
        Configuration = configuration;
    }
}

internal class ConfigurationHolder
{
    public IConfiguration Configuration { get; }

    public ConfigurationHolder(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}