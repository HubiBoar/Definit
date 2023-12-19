namespace Explicit.Validation.NewFluent.Fluent;

public sealed class FluentValidator<TValue> : AbstractValidator<TValue>
    where TValue : IValidate<TValue>
{
    internal FluentValidator()
    {
    }
}
