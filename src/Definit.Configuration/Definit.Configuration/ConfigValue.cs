
namespace Definit.Configuration;

public interface IConfigValue : ISectionName, IConfigObject
{

}

public interface IConfigValue<TValue> : IValidate<TValue>, IConfigValue
{
}

public abstract class ConfigValueBase<TSelf, TValue, TMethod> : IConfigObject<Value<TValue, TMethod>>
    where TSelf : ISectionName
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    public delegate IsValid<Value<TValue, TMethod>> Get();

    public static OneOf<Success, ValidationErrors> Validate(Validator<TValue> context)
    {
        return TMethod.Validate(context);
    }

    public static IsValid<Value<TValue, TMethod>> Create(IServiceProvider _, IConfiguration configuration)
    {
        return Create(configuration);
    }

    public static OneOf<Success, ValidationErrors> Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Get>(provider => () => Create(configuration));

        return Create(configuration).Success;
    }

    public static IsValid<Value<TValue, TMethod>> Create(IConfiguration configuration)
    {
        return ConfigHelper.GetValue<TValue>(configuration, TSelf.SectionName)
            .Match(
                value => new Value<TValue, TMethod>(value).IsValid(),
                IsValid<Value<TValue, TMethod>>.Error);
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

public static class ConfigValue<TValue, TMethod>
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    public abstract class As<TSelf> : ConfigValue<TSelf, TValue, TMethod>
        where TSelf : As<TSelf>, new()
    {
    }
}