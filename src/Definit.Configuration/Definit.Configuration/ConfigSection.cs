namespace Definit.Configuration;

public abstract class ConfigSectionBase<TSelf> : IConfigObject<TSelf>
    where TSelf : ConfigSectionBase<TSelf>, ISectionName, IValidate<TSelf>
{
    public delegate IsValid<TSelf> Get();

    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Get>(provider => () => Create(configuration));
    }

    public static IsValid<TSelf> Create(IConfiguration configuration)
    {
        return ConfigHelper.GetValue<TSelf, TSelf>(configuration)
            .Match(
                value => value.IsValid(),
                IsValid<TSelf>.Error);
    }

    public static OneOf<Success, ValidationErrors> ValidateConfiguration(IConfiguration configuration)
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