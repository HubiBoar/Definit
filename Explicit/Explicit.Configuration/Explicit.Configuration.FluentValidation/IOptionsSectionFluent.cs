using Explicit.Validation;
using Explicit.Validation.FluentValidation;

namespace Explicit.Configuration.FluentValidation;

public interface IOptionsSectionFluent<TSelf> : IOptionsSectionBase<TSelf>, IFluentValidatable<TSelf>
    where TSelf : class, IOptionsSectionFluent<TSelf>
{
}

public static class FluentValidationHelper
{
    public static OneOf<Success, ValidationErrors> FluentValidation<TOptions>(this TOptions options, Action<FluentValidator<TOptions>> validation)
        where TOptions : class, IOptionsSection<TOptions>
    {
        return FluentValidator.Validate(options, validation);
    }
}