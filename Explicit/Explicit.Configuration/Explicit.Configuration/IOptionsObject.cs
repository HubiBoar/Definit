using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Explicit.Configuration;

public interface IOptionsObject : IValidatable
{
    public static abstract string SectionName { get; }
    
    public virtual SectionValue ConvertToSection()
    {
        return new SectionValue(this);
    }

    internal void SetValue(IConfigurationSection configuration);
}

public interface IOptionsConfiguration<in TConfigType>
    where TConfigType : class
{
    public static abstract void Configure<TOptions>(
        OptionsBuilder<TOptions> configure,
        IConfigurationSection configuration)
        where TOptions : class, TConfigType;
}