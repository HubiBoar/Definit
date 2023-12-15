using Explicit.Validation.FluentValidation;

namespace Explicit.Configuration.FluentValidation;

public interface IOptionsSectionFluent<TSelf> : IOptionsSection<TSelf>, IFluentValidatable<TSelf>
    where TSelf : class, IOptionsSectionFluent<TSelf>
{
}