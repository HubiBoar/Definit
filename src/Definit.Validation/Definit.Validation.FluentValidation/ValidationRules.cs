namespace Definit.Validation.FluentValidation;

public static class ValidationRules
{
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