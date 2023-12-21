using Explicit.Validation;
using Microsoft.Extensions.Configuration;

namespace Explicit.Configuration;

public record SectionValue(object Value);

public interface IConfigObject<TSelf> : IValidate<TSelf>
    where TSelf : IConfigObject<TSelf>
{
    public static abstract string SectionName { get; }

    public static abstract SectionValue ConvertToSection(TSelf value);
    
    public static abstract IsValid<TSelf> GetFromConfiguration(IConfigurationSection section);
}

public interface IConfigSection<TSelf> : IConfigObject<TSelf>
    where TSelf : IConfigSection<TSelf>
{
    static IsValid<TSelf> IConfigObject<TSelf>.GetFromConfiguration(IConfigurationSection section)
    {
        return section.Get<TSelf>()!.IsValid();
    }

    static SectionValue IConfigObject<TSelf>.ConvertToSection(TSelf value)
    {
        return new SectionValue(value);
    }
}