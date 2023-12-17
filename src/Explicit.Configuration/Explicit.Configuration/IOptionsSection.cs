using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Explicit.Configuration;

public sealed record SectionValue(object Value);

public interface IOptionsSection<in TSelf> : IOptionsObject, IOptionsConfiguration<TSelf>
    where TSelf : class, IOptionsSection<TSelf>
{
    static void IOptionsConfiguration<TSelf>.Configure<TOptions>(
        OptionsBuilder<TOptions> configure,
        IConfigurationSection configuration)
    {
        configure.Configure(configuration.Bind);
    }

    void IOptionsObject.SetValue(IConfigurationSection configuration)
    {
        configuration.Bind(this);
    }
}