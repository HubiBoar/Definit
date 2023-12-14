using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Explicit.Configuration;

public sealed record SectionValue(object Value);

public interface IOptionsSectionBase<TSelf> : IOptionsObject, IOptionsConfiguration<TSelf>
    where TSelf : class, IOptionsSectionBase<TSelf>
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

public interface IOptionsSection<TSelf> : IOptionsSectionBase<TSelf>, IValidationMethod<TSelf>
    where TSelf : class, IOptionsSection<TSelf>
{
    OneOf<Success, ValidationErrors> IValidatable.Validate()
    {
        return TSelf.Validate((TSelf)this);
    }
}