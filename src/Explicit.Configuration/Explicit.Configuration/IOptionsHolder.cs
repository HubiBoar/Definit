using Explicit.Validation;
using Microsoft.Extensions.Options;

namespace Explicit.Configuration;

public interface IOptionsHolder<TOptions>
    where TOptions : class, IOptionsObject
{
    IsValid<TOptions> GetValue();
}

internal sealed class OptionsHolder<TOptions> : IOptionsHolder<TOptions>
    where TOptions : class, IOptionsObject
{
    private readonly IOptionsMonitor<TOptions> _monitor;
    
    public OptionsHolder(IOptionsMonitor<TOptions> monitor)
    {
        _monitor = monitor;
    }

    public IsValid<TOptions> GetValue()
    {
        return _monitor.CurrentValue.IsValid();
    }
}