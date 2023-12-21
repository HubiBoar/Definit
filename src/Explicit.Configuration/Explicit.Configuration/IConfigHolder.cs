using Explicit.Validation;
using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigHolder<TSection>
    where TSection : IConfigObject<TSection>
{
    IsValid<TSection> GetValid();
}

internal sealed class ConfigHolder<TSection> : IConfigHolder<TSection>
    where TSection : IConfigObject<TSection>
{
    public ConfigurationHolder Configuration { get; }

    public ConfigHolder(ConfigurationHolder configuration)
    {
        Configuration = configuration;
    }

    public IsValid<TSection> GetValid()
    {
        return TSection.CreateSection(Configuration.Configuration.GetSection(TSection.SectionName));
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