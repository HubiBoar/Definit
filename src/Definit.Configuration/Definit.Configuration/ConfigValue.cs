
namespace Definit.Configuration;

public interface IConfigValue<TValue> : IValidate<TValue>, ISectionName, IConfigObject
{
}

public abstract class ConfigValueBase<TSelf, TValue, TMethod> : IConfigObject<Value<TValue, TMethod>>
    where TSelf : ISectionName
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    public delegate IsValid<Value<TValue, TMethod>> Get();

    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Get>(provider => () => Create(configuration));
    }

    public static IsValid<Value<TValue, TMethod>> Create(IConfiguration configuration)
    {
        return ConfigHelper.GetValue<TValue, TSelf>(configuration)
            .Match(
                value => new Value<TValue, TMethod>(value).IsValid(),
                IsValid<Value<TValue, TMethod>>.Error);
    }

    public static OneOf<Success, ValidationErrors> Validate(Validator<TValue> context)
    {
        return TMethod.Validate(context);
    }

    public static OneOf<Success, ValidationErrors> ValidateConfiguration(IConfiguration configuration)
    {
        return Create(configuration).Success;
    }
}

public abstract class ConfigValue<TSelf, TValue, TMethod> : ConfigValueBase<TSelf, TValue, TMethod>, ISectionName, IConfigValue<TValue> 
    where TSelf : ConfigValue<TSelf, TValue, TMethod>, new()
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    static string ISectionName.SectionName => new TSelf().SectionName;

    protected abstract string SectionName { get; }
}