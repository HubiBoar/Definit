namespace Definit.Configuration;

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
}

public abstract class ConfigValue<TSelf, TValue, TMethod> : ConfigValueBase<TSelf, TValue, TMethod>, ISectionName
    where TSelf : ConfigValue<TSelf, TValue, TMethod>, new()
    where TValue : notnull
    where TMethod : IValidate<TValue>
{
    static string ISectionName.SectionName => new TSelf().SectionName;

    protected abstract string SectionName { get; }
}