
namespace Definit.Configuration;

public abstract class ConfigSectionBase<TSelf> : IConfigObject<TSelf>
    where TSelf : ConfigSectionBase<TSelf>, ISectionName, IValidate<TSelf>
{
    public delegate IsValid<TSelf> Get();

    public static IsValid<TSelf> Create(IConfiguration configuration)
    {
        return ConfigHelper.GetValue<TSelf>(configuration, TSelf.SectionName)
            .Match(
                value => value.IsValid(),
                error => error,
                error => error);
    }

    public static IsValid<TSelf> Create(IServiceProvider _, IConfiguration configuration)
    {
        return Create(configuration);
    }

    public static ValidationResult Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Get>(provider => () => Create(configuration));

        return Create(configuration).Success;
    }

    public static ValidationResult ValidateConfiguration(IConfiguration configuration)
    {
        return Create(configuration).Success;
    }
}

public abstract class ConfigSection<TSelf> : ConfigSectionBase<TSelf>, ISectionName, IValidate<TSelf>
    where TSelf : ConfigSection<TSelf>, new()
{
    protected abstract string SectionName { get; }
    protected abstract ValidationResult Validate(Validator<TSelf> context);

    static string ISectionName.SectionName => new TSelf().SectionName;
    static ValidationResult IValidate<TSelf>.Validate(Validator<TSelf> context) => new TSelf().Validate(context);
}

public static class ConfigSection
{
    public abstract class As<TSelf> : ConfigSection<TSelf>
        where TSelf : As<TSelf>, new()
    {
    }
}