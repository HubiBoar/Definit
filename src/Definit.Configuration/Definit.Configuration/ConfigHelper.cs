namespace Definit.Configuration;

public interface IConfigObject
{
    public static abstract OneOf<Success, ValidationErrors> Register(IServiceCollection services, IConfiguration configuration);

    public static abstract OneOf<Success, ValidationErrors> ValidateConfiguration(IConfiguration configuration);
}

public interface IConfigObject<TValue> : IConfigObject
    where TValue : IValidate<TValue>
{
    public static abstract IsValid<TValue> Create(IServiceProvider services, IConfiguration configuration);
}

public interface ISectionName
{
    public abstract static string SectionName { get; }
}

public static class ConfigHelper
{
    public static OneOf<TValue, ValidationErrors> GetValue<TValue>(IConfiguration configuration, string sectionName)
    {
        var section = configuration.GetSection(sectionName);
        if(section.Exists() == false)
        {
            return new ValidationErrors($"Section: [{sectionName}] Is Missing");
        }

        var value = section.Get<TValue>();
        if (value is null)
        {
            return ValidationErrors.Null(DefinitType.GetTypeVerboseName<TValue>());
        }

        return value;
    }

    public static OneOf<Success, ValidationErrors> Register<T>(this IServiceCollection services, IConfiguration configuration)
        where T : IConfigObject
    {
        return T.Register(services, configuration);
    }
}