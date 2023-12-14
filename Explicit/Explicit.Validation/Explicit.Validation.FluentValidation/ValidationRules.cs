namespace Explicit.Validation.FluentValidation;

public static class ValidationRules
{
    public static void UseValidationMethod<TFrom, TMethod>(this RuleBuilder<TFrom, string> rule)
        where TMethod : IFluentValidationRule<string>
    {
        TMethod.SetupValidation(rule);
    }

    public static void UseValidationMethod<TFrom, TMethod, TValue>(this RuleBuilder<TFrom, TValue> rule)
        where TMethod : IFluentValidationRule<TValue>
    {
        TMethod.SetupValidation(rule);
    }
    
    public static void IsConnectionString<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsClientId<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsCron<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(2);
    }

    public static void IsUrl<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsKey<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsSendGridTemplateId<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsDomainName<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsContainerName<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }

    public static void IsSecret<TFrom>(this IRuleBuilder<TFrom, string> rule)
    {
        rule.NotEmpty().MinimumLength(4);
    }
}