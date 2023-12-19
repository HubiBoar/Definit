namespace Explicit.Validation.NewFluent.Configuration;

public interface IConfigValue<in TValue, TMethod> : ISectionName, IValidationRule<TValue>
    where TValue : notnull
    where TMethod : IValidationRule<TValue>
{
    static void IValidationRule<TValue>.SetupRule<TFrom>(IRuleBuilder<TFrom, TValue> ruleBuilder)
    {
        TMethod.SetupRule(ruleBuilder);
    }
}