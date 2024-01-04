using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Explicit.Configuration;

public record SectionValue(object Value);

public interface IConfigObject<TSelf> : IValidate<TSelf>
    where TSelf : IConfigObject<TSelf>
{
    public static abstract string SectionName { get; }

    public static abstract SectionValue ConvertToSection(TSelf value);

    public static abstract void RegisterDepedencies(IServiceCollection services);
    
    public static abstract IsValid<TSelf> GetFromConfiguration(IServiceProvider provider, IConfigurationSection section);
}

public interface IConfigSection<TSelf> : IConfigObject<TSelf>
    where TSelf : IConfigSection<TSelf>
{
    static IsValid<TSelf> IConfigObject<TSelf>.GetFromConfiguration(IServiceProvider provider, IConfigurationSection section)
    {
        return section.Get<TSelf>()!.IsValid();
    }

    static void IConfigObject<TSelf>.RegisterDepedencies(IServiceCollection services)
    {
    }

    static SectionValue IConfigObject<TSelf>.ConvertToSection(TSelf value)
    {
        return new SectionValue(value);
    }
}