using Explicit.Validation;
using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public interface IConfigObject<TSelf> : IValidate<TSelf>
    where TSelf : IConfigObject<TSelf>
{
    public static abstract string SectionName { get; }

    public static abstract IsValid<TSelf> CreateSection(IConfigurationSection section);
}

public interface IConfigSection<TSelf> : IConfigObject<TSelf>
    where TSelf : IConfigSection<TSelf>
{
    static IsValid<TSelf> IConfigObject<TSelf>.CreateSection(IConfigurationSection section)
    {
        return section.Get<TSelf>()!.IsValid();
    }
}