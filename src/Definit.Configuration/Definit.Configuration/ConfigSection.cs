
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
                IsValid<TSelf>.Error);
    }

    public static IsValid<TSelf> Create(IServiceProvider _, IConfiguration configuration)
    {
        return Create(configuration);
    }

    public static OneOf<Success, ValidationErrors> Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Get>(provider => () => Create(configuration));

        return Create(configuration).Success;
    }

    public static OneOf<Success, ValidationErrors> IsValid(IConfiguration configuration)
    {
        return Create(configuration).Success;
    }
}

public abstract class ConfigSection<TSelf> : ConfigSectionBase<TSelf>, ISectionName, IValidate<TSelf>
    where TSelf : ConfigSection<TSelf>, new()
{
    protected abstract string SectionName { get; }
    protected abstract OneOf<Success, ValidationErrors> Validate(Validator<TSelf> context);

    static string ISectionName.SectionName => new TSelf().SectionName;
    static OneOf<Success, ValidationErrors> IValidate<TSelf>.Validate(Validator<TSelf> context) => new TSelf().Validate(context);
}

public static class ConfigSection
{
    public abstract class As<TSelf> : ConfigSection<TSelf>
        where TSelf : As<TSelf>, new()
    {
    }
}