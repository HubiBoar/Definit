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
    private ConfigurationHolder Configuration { get; }

    public ConfigHolder(ConfigurationHolder configuration)
    {
        Configuration = configuration;
    }

    public IsValid<TSection> GetValid()
    {
        return TSection.GetFromConfiguration(Configuration.ServiceProvider, Configuration.Configuration.GetSection(TSection.SectionName));
    }
}

internal class ConfigurationHolder
{
    public IConfiguration Configuration { get; }
    public IServiceProvider ServiceProvider { get; }

    public ConfigurationHolder(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
    }
}